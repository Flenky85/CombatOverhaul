using CombatOverhaul.Guids;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;

namespace CombatOverhaul.Bus
{
    /// <summary>
    /// STR scaling (+ Double Slice en off-hand no finesse) + DEX (ITWF) en off-hand finesse.
    /// </summary>
    internal sealed class AttackDamageScaling :
        IGlobalRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IGlobalSubscriber
    {
        // ======================= Config STR =======================
        private const float SingleMain_PerPoint = 0.10f;
        private const float DualPrimary_PerPoint = 0.10f;
        private const float DualOffhand_PerPoint = 0.05f;

        private const int Naturals_WithManufactured_Offset = 2;
        private const bool ExcludePrecision = false;

        private static readonly float[] NaturalPct = {
            0.00f,  // 0
            0.20f,  // 1 natural
            0.10f,  // 2
            0.033f, // 3
            0.025f, // 4
            0.02f,  // 5
            0.016f, // 6
            0.014f, // 7
            0.0125f,// 8
            0.011f, // 9
            0.01f   // 10+
        };

        // ======================= Blueprints =======================
        private static BlueprintFeature _doubleSliceFeat;
        private static BlueprintFeature DoubleSliceFeat =>
            _doubleSliceFeat ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.DoubleSlice);

        private static BlueprintFeature _improvedTWF;
        private static BlueprintFeature ImprovedTWF =>
            _improvedTWF ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.ImprovedTwoWeaponFighting);

        private static BlueprintFeature _greaterTWF;
        private static BlueprintFeature GreaterTWF =>
            _greaterTWF ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.GreaterTwoWeaponFighting);

        // ======================= Contexto =======================
        private struct AttackCtx
        {
            public UnitEntityData Attacker;
            public ItemEntityWeapon AttackWeapon;

            public ItemEntityWeapon PrimaryWeapon;
            public ItemEntityWeapon OffWeapon;

            public bool IsMainHand;
            public bool IsOffHand;

            public bool IsManufacturedHit;
            public bool IsNaturalHit;

            public bool PrimaryIsManufactured;
            public bool OffIsManufactured;
            public bool AnyManufacturedEquipped;

            public int StrMod;
            public int DexMod;
        }

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (evt == null) return;

            try
            {
                var ctx = BuildContext(evt);
                if (ctx.Attacker == null || ctx.AttackWeapon == null) return;

                // ===== STR: per-point =====
                int strPercent = 0;
                if (ctx.StrMod > 0)
                {
                    float perPoint = PerPointFromSTR(ctx); // incluye Double Slice en off-hand no finesse
                    if (perPoint > 0f)
                        strPercent = RoundPct(ctx.StrMod * perPoint * 100f);
                }

                // ===== DEX: ITWF off-hand finesse (2.5% * DEX_mod) =====
                int dexPercent = 0;
                if (ctx.IsOffHand && IsFinesseWeapon(ctx.AttackWeapon) && ctx.DexMod > 0)
                {
                    float perPoint = 0f;
                    if (HasGreaterTWF(ctx.Attacker)) perPoint = 0.05f;   // GTWF
                    else if (HasImprovedTWF(ctx.Attacker)) perPoint = 0.025f;  // ITWF

                    if (perPoint > 0f)
                        dexPercent = RoundPct(ctx.DexMod * perPoint * 100f);
                }

                int total = strPercent + dexPercent;
                if (total == 0) return;

                var bundle = evt.ParentRule?.DamageBundle;
                if (bundle == null) return;

                foreach (var d in bundle)
                {
                    if (d == null) continue;
                    if (d.Type != DamageType.Physical) continue;
                    if (ExcludePrecision && d.Precision) continue;
                    d.BonusPercent += total;
                }
            }
            catch
            {
                // silencio por simplicidad
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) { /* no-op */ }

        // ======================= Build Context =======================
        private static AttackCtx BuildContext(RuleCalculateDamage evt)
        {
            var ctx = new AttackCtx();

            var attacker = evt?.Initiator;
            var parentRule = evt?.ParentRule;
            var attackRoll = parentRule?.AttackRoll;
            var weapon = attackRoll?.Weapon;

            ctx.Attacker = attacker;
            ctx.AttackWeapon = weapon;

            var body = attacker?.Body;
            var primary = body?.PrimaryHand?.MaybeWeapon;
            var off = body?.SecondaryHand?.MaybeWeapon;

            ctx.PrimaryWeapon = primary;
            ctx.OffWeapon = off;

            // Mano por instancia
            ctx.IsMainHand = object.ReferenceEquals(primary, weapon);
            ctx.IsOffHand = object.ReferenceEquals(off, weapon);

            // Tipo de golpe
            ctx.IsNaturalHit = IsNaturalWeapon(weapon);
            ctx.IsManufacturedHit = IsManufacturedWeapon(weapon);

            ctx.PrimaryIsManufactured = IsManufacturedWeapon(primary);
            ctx.OffIsManufactured = IsManufacturedWeapon(off);
            ctx.AnyManufacturedEquipped = ctx.PrimaryIsManufactured || ctx.OffIsManufactured;

            ctx.StrMod = attacker?.Stats?.Strength?.Bonus ?? 0;
            ctx.DexMod = attacker?.Stats?.Dexterity?.Bonus ?? 0;

            return ctx;
        }

        // ======================= STR (incluye Double Slice) =======================
        private static float PerPointFromSTR(AttackCtx ctx)
        {
            if (ctx.IsNaturalHit)
            {
                int naturals = CountNaturalWeapons(ctx.Attacker, ctx.AttackWeapon, ctx.AnyManufacturedEquipped);
                if (naturals <= 0) naturals = 1;

                naturals = ctx.AnyManufacturedEquipped
                    ? Math.Min(naturals + Naturals_WithManufactured_Offset, 10)
                    : Math.Min(naturals, 10);

                return NaturalPct[naturals];
            }

            if (ctx.IsManufacturedHit)
            {
                // Single-wield
                if (ctx.PrimaryIsManufactured && !ctx.OffIsManufactured)
                    return SingleMain_PerPoint;

                // Dual-wield
                if (ctx.PrimaryIsManufactured && ctx.OffIsManufactured)
                {
                    float perPoint = ctx.IsOffHand ? DualOffhand_PerPoint : DualPrimary_PerPoint;

                    // Double Slice: +5%/punto en off-hand si NO es finesse y el portador tiene el feat
                    if (ctx.IsOffHand && HasDoubleSlice(ctx.Attacker) && !IsFinesseWeapon(ctx.AttackWeapon))
                        perPoint += 0.05f;

                    return perPoint;
                }

                // Caso borde
                return SingleMain_PerPoint;
            }

            return 0f;
        }

        // ======================= Helpers feats / armas =======================
        private static bool HasDoubleSlice(UnitEntityData unit)
        {
            if (unit == null || DoubleSliceFeat == null) return false;
            return unit.Descriptor != null && unit.Descriptor.HasFact(DoubleSliceFeat);
        }

        private static bool HasImprovedTWF(UnitEntityData unit)
        {
            if (unit == null || ImprovedTWF == null) return false;
            return unit.Descriptor != null && unit.Descriptor.HasFact(ImprovedTWF);
        }

        private static bool HasGreaterTWF(UnitEntityData unit)
        {
            if (unit == null || GreaterTWF == null) return false;
            return unit.Descriptor != null && unit.Descriptor.HasFact(GreaterTWF);
        }

        private static bool IsFinesseWeapon(ItemEntityWeapon w)
        {
            var bp = w?.Blueprint;
            if (bp == null) return false;

            if (bp.IsLight) return true;

            var cat = bp.Category;
            return cat == WeaponCategory.ElvenCurvedBlade
                || cat == WeaponCategory.Estoc
                || cat == WeaponCategory.Rapier;
        }

        private static bool IsNaturalWeapon(ItemEntityWeapon w)
        {
            var bp = w?.Blueprint;
            return bp != null && (bp.IsNatural || bp.IsUnarmed);
        }

        private static bool IsManufacturedWeapon(ItemEntityWeapon w)
        {
            var bp = w?.Blueprint;
            return w != null && !(bp?.IsNatural ?? false) && !(bp?.IsUnarmed ?? false);
        }

        private static int CountNaturalWeapons(UnitEntityData unit, ItemEntityWeapon current, bool anyManufacturedEquipped)
        {
            if (unit?.Body == null) return 0;

            int count = 0;
            bool hasUnarmedSomewhere = false;

            ScanSlot(unit.Body.PrimaryHand, ref count, ref hasUnarmedSomewhere);
            ScanSlot(unit.Body.SecondaryHand, ref count, ref hasUnarmedSomewhere);

            var extra = unit.Body.AdditionalLimbs;
            if (extra != null)
            {
                foreach (var slot in extra)
                    ScanSlot(slot, ref count, ref hasUnarmedSomewhere);
            }

            bool currentIsUnarmed = current?.Blueprint?.IsUnarmed ?? false;
            if (hasUnarmedSomewhere)
            {
                if (!anyManufacturedEquipped || currentIsUnarmed)
                    count++;
            }

            return count;

            void ScanSlot(WeaponSlot slot, ref int c, ref bool hasUnarmed)
            {
                var w = slot?.MaybeWeapon;
                var bp = w?.Blueprint;
                if (bp == null) return;

                if (bp.IsUnarmed) { hasUnarmed = true; return; }
                if (bp.IsNatural) c++;
            }
        }

        // ======================= Util =======================
        private static int RoundPct(float x)
        {
            return (int)Math.Round(x, MidpointRounding.AwayFromZero);
        }
    }
}
