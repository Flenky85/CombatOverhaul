using System;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class ArmorCalculator
    {
        public static int GetArmorBase(ItemEntityArmor armorItem)
        {
            if (!(armorItem?.Blueprint is BlueprintItemArmor bp)) return 0;

            var fromType = SafeArmorBonusFromType(bp);
            if (fromType > 0) return fromType;

            return SafeArmorBonusFromItem(bp);
        }

        public static float GetBaseRdPercentFromArmorBase(int armorBase)
        {
            if (armorBase <= 0) return 0f;
            return armorBase * 0.05f;
        }

        public static int ComputeArmorDrDisplayPercent(ItemEntityArmor armor)
        {
            var bp = armor?.Blueprint as BlueprintItemArmor;
            var baseReal = bp?.Type?.ArmorBonus ?? 0;
            return baseReal > 0 ? baseReal * 5 : 0;
        }

        public static int GetArmorMaxDex(ItemEntityArmor armor)
        {
            if (!(armor?.Blueprint is BlueprintItemArmor bp)) return 6;

            var fromItem = SafeMaxDexFromItem(bp);
            if (fromItem > 0) return fromItem;

            var fromType = SafeMaxDexFromType(bp);
            if (fromType > 0) return fromType;

            return 6;
        }

        public static int ComputeAcReductionPercentFromMaxDex(int maxDex)
        {
            var p = 27 - (3 * maxDex);
            if (p < 0) p = 0;
            if (p > 27) p = 27;
            return p;
        }

        public static bool IsPhysical(DamageValue dv) => dv.Source is PhysicalDamage;

        private static int SafeArmorBonusFromType(BlueprintItemArmor bp)
            => bp?.Type?.ArmorBonus ?? 0;

        private static int SafeArmorBonusFromItem(BlueprintItemArmor bp)
            => bp?.ArmorBonus ?? 0;

        private static int SafeMaxDexFromItem(BlueprintItemArmor bp)
        {
            return (int?)bp?.MaxDexterityBonus ?? 0;
        }

        private static int SafeMaxDexFromType(BlueprintItemArmor bp)
            => (int?)(bp?.Type?.MaxDexterityBonus) ?? 0;
    }
}
