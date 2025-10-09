using CombatOverhaul.Guids;
using CombatOverhaul.Patches.Features.Commons;
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
    internal sealed class StrengthPercentPerPoint :
        IGlobalRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IGlobalSubscriber
    {
        private const float SingleMain_PerPoint = 0.10f;
        private const float DualPrimary_PerPoint = 0.10f;
        private const float DualOffhand_PerPoint = 0.05f;

        private const int Naturals_WithManufactured_Offset = 2;
        private const bool ExcludePrecision = false;

        private static readonly float[] NaturalPct = {
            0.00f,  
            0.20f,  
            0.10f,  
            0.033f,  
            0.025f, 
            0.02f,  
            0.016f,  
            0.014f,
            0.0125f,
            0.011f,
            0.01f   
        };

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (evt == null) return;

            try
            {
                var attacker = evt.Initiator;
                var parentRule = evt.ParentRule;
                var attackRoll = parentRule?.AttackRoll;
                if (attacker == null || attackRoll == null) return;

                var weapon = attackRoll.Weapon;
                if (weapon == null) return;

                bool isNaturalHit = IsNaturalWeapon(weapon);
                bool isManufacturedHit = IsManufacturedWeapon(weapon);

                var body = attacker.Body;
                var primary = body?.PrimaryHand?.MaybeWeapon;
                var off = body?.SecondaryHand?.MaybeWeapon;

                bool primaryIsManufactured = IsManufacturedWeapon(primary);
                bool offIsManufactured = IsManufacturedWeapon(off);
                bool anyManufacturedEquipped = primaryIsManufactured || offIsManufactured;

                bool isOffhandHit = off == weapon;

                int strMod = attacker.Stats?.Strength?.Bonus ?? 0;

                int dexBonusPercent = (isOffhandHit && primaryIsManufactured && offIsManufactured)
                    ? GetImprovedTWFDexBonusPercent(attacker, weapon)
                    : 0;

                if (strMod == 0 && dexBonusPercent == 0) return;

                float perPoint;
                if (isManufacturedHit)
                {
                    perPoint = ResolveManufacturedPerPoint(primaryIsManufactured, offIsManufactured, isOffhandHit);

                    if (primaryIsManufactured && offIsManufactured && isOffhandHit)
                        perPoint = AddDoubleSliceBonus(attacker, weapon, perPoint);
                }
                else if (isNaturalHit)
                {
                    int naturals = CountNaturalWeapons(attacker, weapon, anyManufacturedEquipped);
                    if (naturals <= 0) naturals = 1;

                    naturals = anyManufacturedEquipped
                        ? Math.Min(naturals + Naturals_WithManufactured_Offset, 10)
                        : Math.Min(naturals, 10);

                    perPoint = NaturalPct[naturals];
                }
                else
                {
                    if (dexBonusPercent == 0) return;
                    perPoint = 0f;
                }

                int extraPercentStr = 0;
                if (strMod != 0 && perPoint != 0f)
                    extraPercentStr = (int)Math.Round(strMod * perPoint * 100f, MidpointRounding.AwayFromZero);

                int totalPercent = extraPercentStr + dexBonusPercent;
                if (totalPercent == 0) return;

                var bundle = parentRule.DamageBundle;
                if (bundle == null) return;

                foreach (var d in bundle)
                {
                    if (d == null) continue;
                    if (d.Type != DamageType.Physical) continue;
                    if (ExcludePrecision && d.Precision) continue;
                    d.BonusPercent += totalPercent;
                }
            }
            catch (Exception) { }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) { /* no-op */ }

        private static float ResolveManufacturedPerPoint(bool primaryManu, bool offManu, bool isOffhandHit)
        {
            if (primaryManu && !offManu) return SingleMain_PerPoint;               
            if (primaryManu && offManu) return isOffhandHit ? DualOffhand_PerPoint : DualPrimary_PerPoint; 
            return SingleMain_PerPoint;                                            
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
                foreach (var slot in extra) ScanSlot(slot, ref count, ref hasUnarmedSomewhere);
            }

            bool currentIsUnarmed = current?.Blueprint?.IsUnarmed ?? false;
            if (hasUnarmedSomewhere)
            {
                if (!anyManufacturedEquipped || currentIsUnarmed)
                    count++; 
            }

            return count;

            static void ScanSlot(WeaponSlot slot, ref int c, ref bool hasUnarmed)
            {
                var w = slot?.MaybeWeapon;
                var bp = w?.Blueprint;
                if (bp == null) return;

                if (bp.IsUnarmed) { hasUnarmed = true; return; }  
                if (bp.IsNatural) c++;
            }


        }

        private static BlueprintFeature _doubleSliceFeat;
        private static BlueprintFeature DoubleSliceFeat =>
            _doubleSliceFeat ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.DoubleSlice);

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

        private static float AddDoubleSliceBonus(UnitEntityData attacker, ItemEntityWeapon weapon, float basePerPoint)
        {
            if (attacker != null
                && DoubleSliceFeat != null
                && (attacker.Descriptor?.HasFact(DoubleSliceFeat) ?? false)
                && !IsFinesseWeapon(weapon))
            {
                return basePerPoint + 0.05f;
            }
            return basePerPoint;
        }

        private static BlueprintFeature _improvedTWF;
        private static BlueprintFeature ImprovedTWF =>
            _improvedTWF ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.ImprovedTwoWeaponFighting);

        private static BlueprintFeature _greaterTWF;
        private static BlueprintFeature GreaterTWF =>
            _greaterTWF ??= ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.GreaterTwoWeaponFighting);

        private static int GetImprovedTWFDexBonusPercent(UnitEntityData attacker, ItemEntityWeapon weapon)
        {
            if (attacker == null || weapon == null) return 0;
            if (!IsFinesseWeapon(weapon)) return 0;

            float perPoint;
            if (GreaterTWF != null && (attacker.Descriptor?.HasFact(GreaterTWF) ?? false))
                perPoint = 0.05f;
            else if (ImprovedTWF != null && (attacker.Descriptor?.HasFact(ImprovedTWF) ?? false))
                perPoint = 0.025f;
            else
                return 0;

            int dexMod = attacker.Stats?.Dexterity?.Bonus ?? 0;
            if (dexMod <= 0) return 0;

            return (int)Math.Round(dexMod * perPoint * 100f, MidpointRounding.AwayFromZero);
        }
    }
}
