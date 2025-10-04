using System;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class ArmorCalculator
    {
        // --- Lectura base de la armadura (sin reflexión) ---
        public static int GetArmorBase(ItemEntityArmor armorItem)
        {
            try
            {
                var bp = armorItem?.Blueprint as BlueprintItemArmor;
                if (bp == null) return 0;

                // Prioriza el bonus del catálogo (Type) si existe; si no, usa el del ítem.
                int fromType = 0;
                try { fromType = bp.Type?.ArmorBonus ?? 0; } catch { /*ignore*/ }

                int fromItem = 0;
                try { fromItem = bp.ArmorBonus; } catch { /*ignore*/ }

                return fromType > 0 ? fromType : fromItem;
            }
            catch { return 0; }
        }

        // --- RD base: 5% por punto de armadura ---
        public static float GetBaseRdPercentFromArmorBase(int armorBase)
        {
            return Math.Max(0f, armorBase * 0.05f);
        }

        // --- RD aplicada según tipo de daño (100% física, 50% resto) ---
        public static float ApplyTypeScaling(float rdBase, bool isPhysical)
        {
            return isPhysical ? rdBase : rdBase * 0.5f;
        }

        // --- UI: porcentaje DR mostrado (baseReal * 5) ---
        public static int ComputeArmorDrDisplayPercent(ItemEntityArmor armor)
        {
            var bpArmor = armor?.Blueprint as BlueprintItemArmor;
            int baseReal = (bpArmor?.Type?.ArmorBonus) ?? 0;
            return Math.Max(0, baseReal * 5);
        }

        // --- Max Dex real del ítem (intenta ítem -> tipo; fallback heurístico) ---
        // Nota: si el blueprint del ítem ya incorpora materiales (p.ej. versiones "mithral"),
        // MaxDexBonus del propio ítem contendrá el +2. Si no, usamos el del Type.
        public static int GetArmorMaxDex(ItemEntityArmor armor)
        {
            try
            {
                if (armor == null) return 0;

                var bp = armor.Blueprint as BlueprintItemArmor;
                if (bp == null) return 0;

                // 1) MaxDex del ítem (preferente: suele reflejar variaciones como mithral)
                int fromItem = 0;
                try { fromItem = GetBlueprintMaxDex(bp); } catch { /*ignore*/ }
                if (fromItem > 0) return fromItem;

                // 2) MaxDex del catálogo/tipo
                int fromType = 0;
                try { fromType = GetTypeMaxDex(bp); } catch { /*ignore*/ }
                if (fromType > 0) return fromType;

                // 3) Fallback conservador (por si algún blueprint carece del dato)
                return 6;
            }
            catch { return 6; }
        }

        // --- Única función canónica para el % de reducción universal de AC desde Max Dex ---
        // Fórmula del diseño: 27 - 3*MaxDex, clamp 0..27
        public static int ComputeAcReductionPercentFromMaxDex(int maxDex)
        {
            int p = 27 - 3 * maxDex;
            if (p < 0) p = 0;
            if (p > 27) p = 27;
            return p;
        }

        // --- Utilidad: ¿es físico? ---
        public static bool IsPhysical(DamageValue dv)
        {
            return dv.Source is PhysicalDamage;
        }

        // ===== Helpers privados =====

        // Lee MaxDex en el propio blueprint del ítem si existe
        private static int GetBlueprintMaxDex(BlueprintItemArmor bp)
        {
            // Algunos builds exponen MaxDexBonus en el item directamente
            try
            {
                // Si tu build no tiene esta propiedad pública, este try devolverá 0 y pasaremos al Type.
                return (int)bp.MaxDexterityBonus;
            }
            catch { /* ignore */ }

            return 0;
        }

        // Lee MaxDex en el Type (catálogo). Útil como fallback.
        private static int GetTypeMaxDex(BlueprintItemArmor bp)
        {
            try { return (int)(bp.Type?.MaxDexterityBonus ?? 0); }
            catch { return 0; }
        }
    }
}
