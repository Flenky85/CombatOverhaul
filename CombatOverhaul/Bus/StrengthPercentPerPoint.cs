using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using System;

namespace CombatOverhaul.Bus
{
    internal sealed class StrengthPercentPerPoint :
        IGlobalRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IGlobalSubscriber
    {
        private const float SingleMain_PerPoint = 0.15f;
        private const float DualPrimary_PerPoint = 0.15f;
        private const float DualOffhand_PerPoint = 0.15f;

        private const int Naturals_WithManufactured_Offset = 2;
        private const bool ExcludePrecision = false;

        private static readonly float[] NaturalPct = {
            0.00f,  
            0.30f,  
            0.15f,  
            0.10f,  
            0.075f, 
            0.06f,  
            0.05f,  
            0.0428f,
            0.0375f,
            0.0333f,
            0.03f   
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

                int strMod = attacker.Stats?.Strength?.Bonus ?? 0;
                if (strMod == 0) return;

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

                float perPoint;
                if (isManufacturedHit)
                {
                    perPoint = ResolveManufacturedPerPoint(primaryIsManufactured, offIsManufactured, isOffhandHit);
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
                    return;
                }

                if (perPoint == 0f) return;

                int extraPercent = (int)Math.Round(strMod * perPoint * 100f, MidpointRounding.AwayFromZero);
                if (extraPercent == 0) return;

                var bundle = parentRule.DamageBundle;
                if (bundle == null) return;

                foreach (var d in bundle)
                {
                    if (d == null) continue;
                    if (d.Type != DamageType.Physical) continue; 
                    if (ExcludePrecision && d.Precision) continue;
                    d.BonusPercent += extraPercent;
                }
            }
            catch (Exception) {  }
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
    }
}
