using System;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class ArmorCalculator
    {
        // --- Lectura base de la armadura (sin reflexión) ---
        // Prioriza el bonus del catálogo (Type) si existe; si no, usa el del ítem.
        public static int GetArmorBase(ItemEntityArmor armorItem)
        {
            var bp = armorItem?.Blueprint as BlueprintItemArmor;
            if (bp == null) return 0;

            var fromType = SafeArmorBonusFromType(bp);
            if (fromType > 0) return fromType;

            // Si Type no da valor > 0, usa el del item
            return SafeArmorBonusFromItem(bp);
        }

        // --- RD base: 5% por punto de armadura ---
        public static float GetBaseRdPercentFromArmorBase(int armorBase)
        {
            if (armorBase <= 0) return 0f;
            return armorBase * 0.05f;
        }

        // --- UI: porcentaje DR mostrado (baseReal * 5) ---
        // Usa el ArmorBonus del Type (catálogo) como pedías.
        public static int ComputeArmorDrDisplayPercent(ItemEntityArmor armor)
        {
            var bp = armor?.Blueprint as BlueprintItemArmor;
            var baseReal = bp?.Type?.ArmorBonus ?? 0;
            return baseReal > 0 ? baseReal * 5 : 0;
        }

        // --- Max Dex real del ítem (intenta ítem -> tipo; fallback heurístico) ---
        // Nota: si el blueprint del ítem ya incorpora materiales (mithral...), el MaxDex del propio ítem ya incluye el +2.
        public static int GetArmorMaxDex(ItemEntityArmor armor)
        {
            var bp = armor?.Blueprint as BlueprintItemArmor;
            if (bp == null) return 6; // fallback conservador

            // 1) MaxDex del ítem (preferente: refleja variaciones como mithral)
            var fromItem = SafeMaxDexFromItem(bp);
            if (fromItem > 0) return fromItem;

            // 2) MaxDex del Type (catálogo)
            var fromType = SafeMaxDexFromType(bp);
            if (fromType > 0) return fromType;

            // 3) Fallback conservador
            return 6;
        }

        // --- Única función canónica para el % de reducción universal de AC desde Max Dex ---
        // Fórmula del diseño: 27 - 3*MaxDex, clamp 0..27
        public static int ComputeAcReductionPercentFromMaxDex(int maxDex)
        {
            var p = 27 - (3 * maxDex);
            if (p < 0) p = 0;
            if (p > 27) p = 27;
            return p;
        }

        // --- Utilidad: ¿es físico? ---
        public static bool IsPhysical(DamageValue dv) => dv.Source is PhysicalDamage;

        // ===== Helpers privados =====

        // ArmorBonus del Type, seguro ante nulls
        private static int SafeArmorBonusFromType(BlueprintItemArmor bp)
            => bp?.Type?.ArmorBonus ?? 0;

        // ArmorBonus del propio item, seguro ante nulls
        private static int SafeArmorBonusFromItem(BlueprintItemArmor bp)
            => bp?.ArmorBonus ?? 0;

        // MaxDex expuesto por el item (incluye materiales si el blueprint del item ya los bakea)
        private static int SafeMaxDexFromItem(BlueprintItemArmor bp)
        {
            // Algunas builds exponen MaxDexterityBonus en el item.
            // Si no existe en tu build, devolverá 0 (default del null-coalesce).
            // Nota: el cast a int es seguro porque MaxDex suele ser int/short.
            return (int?)bp?.MaxDexterityBonus ?? 0;
        }

        // MaxDex del Type (catálogo), útil como fallback.
        private static int SafeMaxDexFromType(BlueprintItemArmor bp)
            => (int?)(bp?.Type?.MaxDexterityBonus) ?? 0;
    }
}
