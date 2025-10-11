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
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;

namespace CombatOverhaul.Bus
{
    internal sealed class AttackDamageScaling :
        IGlobalRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IGlobalSubscriber
    {
        private const float SingleMain_PerPoint = 0.10f;
        private const float DualPrimary_PerPoint = 0.10f;
        private const float DualOffhand_PerPoint = 0.05f;

        private const int Naturals_WithManufactured_Offset = 2;
        private const bool ExcludePrecision = false;

        private static readonly float[] NaturalPct = {
            0.00f,  // 0
            0.30f,  // 1 natural
            0.10f,  // 2
            0.066f, // 3
            0.05f,  // 4
        };

        //Feats
        private static BlueprintFeature _doubleSliceFeat;
        private static BlueprintFeature DoubleSliceFeat =>
            _doubleSliceFeat ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.DoubleSlice);

        private static BlueprintFeature _improvedTWF;
        private static BlueprintFeature ImprovedTWF =>
            _improvedTWF ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.ImprovedTwoWeaponFighting);

        private static BlueprintFeature _greaterTWF;
        private static BlueprintFeature GreaterTWF =>
            _greaterTWF ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.GreaterTwoWeaponFighting);

        private static BlueprintFeature _weaponFinesse;
        private static BlueprintFeature WeaponFinesseFeat =>
            _weaponFinesse ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.WeaponFinesse);

        //Buffs
        private static BlueprintBuff _dragonStyleBuff;
        private static BlueprintBuff DragonStyleBuff =>
            _dragonStyleBuff ??= ResourcesLibrary.TryGetBlueprint<BlueprintBuff>(BuffsGuids.DragonStyle);

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
            public bool IsUnarmed;

            public bool PrimaryIsManufactured;
            public bool OffIsManufactured;
            public bool AnyManufacturedEquipped;

            public int StrMod;
            public int DexMod;

            public bool IsFirstAttack;
        }

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (evt == null) return;

            try
            {
                var ctx = BuildContext(evt);
                if (ctx.Attacker == null || ctx.AttackWeapon == null) return;

                int dexBonusPos = Math.Max(0, ctx.DexMod);
                int strBonusPos = Math.Max(0, ctx.StrMod);

                int strPercent = 0;
                
                {
                    float perPoint = PerPointFromSTR(ctx, dexBonusPos, strBonusPos);
                    if (ctx.StrMod != 0 && perPoint != 0f)
                        strPercent = RoundPct(ctx.StrMod * perPoint * 100f); 
                }

                int dexPercent = 0;

                if (ctx.IsOffHand && IsFinesseWeapon(ctx.AttackWeapon) && dexBonusPos > strBonusPos)
                {
                    float perPoint = 0f;
                    if (HasGreaterTWF(ctx.Attacker)) perPoint = 0.05f;
                    else if (HasImprovedTWF(ctx.Attacker)) perPoint = 0.025f;

                    if (ctx.DexMod != 0 && perPoint != 0f)
                        dexPercent = RoundPct(ctx.DexMod * perPoint * 100f);
                }

                if (ctx.IsMainHand && IsFinesseWeapon(ctx.AttackWeapon) && HasWeaponFinesse(ctx.Attacker) && dexBonusPos > strBonusPos && ctx.DexMod != 0)
                {
                    dexPercent += RoundPct(ctx.DexMod * 0.05f * 100f);
                }

                if (ctx.IsNaturalHit && HasWeaponFinesse(ctx.Attacker) && dexBonusPos > strBonusPos && ctx.DexMod != 0)
                {
                    float perPoint = PerPointFromSTR(ctx, dexBonusPos, strBonusPos);
                    if (ctx.DexMod != 0 && perPoint != 0f)
                        dexPercent = RoundPct(ctx.DexMod * perPoint * 100f);
                }

                if (ctx.IsFirstAttack && ctx.IsUnarmed && HasDragonStyle(ctx.Attacker) && ctx.StrMod != 0) 
                {
                    strPercent += RoundPct(ctx.StrMod * 0.05f * 100f);
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
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) { /* no-op */ }

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

            ctx.IsNaturalHit = IsNaturalWeapon(weapon);
            ctx.IsManufacturedHit = IsManufacturedWeapon(weapon);
            ctx.IsUnarmed = IsUnarmedWeapon(weapon);

            ctx.PrimaryIsManufactured = IsManufacturedWeapon(primary);
            ctx.OffIsManufactured = IsManufacturedWeapon(off);
            ctx.AnyManufacturedEquipped = ctx.PrimaryIsManufactured || ctx.OffIsManufactured;

            if (ctx.IsManufacturedHit)
            {
                ctx.IsMainHand = ReferenceEquals(primary, weapon);
                ctx.IsOffHand = ReferenceEquals(off, weapon);
            }
            else
            {
                ctx.IsMainHand = false;
                ctx.IsOffHand = false;
            }

            ctx.StrMod = attacker?.Stats?.Strength?.Bonus ?? 0;
            ctx.DexMod = attacker?.Stats?.Dexterity?.Bonus ?? 0;

            ctx.IsFirstAttack = GetIsFirstAttack(evt);

            return ctx;
        }

        private static float PerPointFromSTR(AttackCtx ctx, int dexBonusPos, int strBonusPos)
        {
            if (ctx.IsNaturalHit)
            {
                int naturals = CountNaturalWeapons(ctx.Attacker);
                if (naturals < 1) naturals = 1;

                int idx = ctx.AnyManufacturedEquipped
                    ? naturals + Naturals_WithManufactured_Offset
                    : naturals;

                if (idx > 4) idx = 4;

                float perPoint = NaturalPct[idx];

                if (HasWeaponFinesse(ctx.Attacker) && dexBonusPos > strBonusPos)
                    perPoint *= 0.5f;

                return perPoint;
            }

            if (ctx.IsManufacturedHit)
            {
                if (ctx.PrimaryIsManufactured && !ctx.OffIsManufactured)
                {
                    float perPoint = SingleMain_PerPoint;

                    if (ctx.IsMainHand && IsFinesseWeapon(ctx.AttackWeapon) && HasWeaponFinesse(ctx.Attacker) && dexBonusPos > strBonusPos)
                        perPoint = Math.Max(0f, perPoint - 0.05f);

                    return perPoint;
                }

                if (ctx.PrimaryIsManufactured && ctx.OffIsManufactured)
                {
                    float perPoint = ctx.IsOffHand ? DualOffhand_PerPoint : DualPrimary_PerPoint;

                    if (ctx.IsOffHand && HasDoubleSlice(ctx.Attacker))
                        perPoint += 0.05f;

                    if (ctx.IsMainHand && IsFinesseWeapon(ctx.AttackWeapon) && HasWeaponFinesse(ctx.Attacker) && dexBonusPos > strBonusPos)
                        perPoint = Math.Max(0f, perPoint - 0.05f);

                    if (ctx.IsOffHand && IsFinesseWeapon(ctx.AttackWeapon) && HasGreaterTWF(ctx.Attacker) && dexBonusPos > strBonusPos)
                        perPoint = Math.Max(0f, perPoint - 0.05f);
                    else if (ctx.IsOffHand && IsFinesseWeapon(ctx.AttackWeapon) && HasImprovedTWF(ctx.Attacker) && dexBonusPos > strBonusPos)
                        perPoint = Math.Max(0f, perPoint - 0.025f);

                    return perPoint;
                }

                return SingleMain_PerPoint;
            }

            return 0f;
        }

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

        private static bool HasWeaponFinesse(UnitEntityData unit)
        {
            if (unit == null || WeaponFinesseFeat == null) return false;
            return unit.Descriptor != null && unit.Descriptor.HasFact(WeaponFinesseFeat);
        }

        private static bool HasDragonStyle(UnitEntityData unit)
        {
            if (unit == null || DragonStyleBuff == null) return false;
            return unit.Descriptor != null && unit.Descriptor.HasFact(DragonStyleBuff);
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
            return w?.Blueprint?.IsNatural ?? false;
        }

        private static bool IsManufacturedWeapon(ItemEntityWeapon w)
        {
            return w != null && !IsNaturalWeapon(w);
        }

        private static bool IsUnarmedWeapon(ItemEntityWeapon w)
        {
            return w?.Blueprint.IsUnarmed ?? false;
        }

        private static int CountNaturalWeapons(UnitEntityData unit)
        {
            if (unit?.Body == null) return 0;

            int count = 0;

            static void Scan(WeaponSlot slot, ref int c)
            {
                var w = slot?.MaybeWeapon;
                if (w?.Blueprint?.IsNatural == true) c++;
            }

            Scan(unit.Body.PrimaryHand, ref count);
            Scan(unit.Body.SecondaryHand, ref count);

            var limbs = unit.Body.AdditionalLimbs;
            if (limbs != null)
                foreach (var s in limbs) Scan(s, ref count);

            return count;
        }

        private static bool GetIsFirstAttack(RuleCalculateDamage evt)
        {
            if (!(evt?.ParentRule is RuleDealDamage rd)) return false;

            var raw = rd.AttackRoll?.RuleAttackWithWeapon;
            if (raw != null)
                return raw.IsFirstAttack;

            return false;
        }

        private static int RoundPct(float x)
        {
            return (int)Math.Round(x, MidpointRounding.AwayFromZero);
        }
    }
}
