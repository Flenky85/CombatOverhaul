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
                var bp = armorItem != null ? armorItem.Blueprint as BlueprintItemArmor : null;
                if (bp == null) return 0;

                // Prioriza el bonus definido en el "Type" (catálogo), si existe.
                int fromType = 0;
                try { fromType = bp.Type != null ? bp.Type.ArmorBonus : 0; } catch { }

                int fromItem = 0;
                try { fromItem = bp.ArmorBonus; } catch { }

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
            var bpArmor = armor != null ? armor.Blueprint as BlueprintItemArmor : null;
            int baseReal = bpArmor != null && bpArmor.Type != null ? bpArmor.Type.ArmorBonus : 0;
            return Math.Max(0, baseReal * 5);
        }

        // --- UI: MaxDex sin reflexión (usa el tipo efectivo que ya contempla mithral) ---
        public static int GetArmorMaxDex(ItemEntityArmor armor)
        {
            if (armor == null) return 0;

            // ItemEntityArmor.ArmorType() ya aplica la lógica de mithral (publicized).
            switch (armor.ArmorType())
            {
                case ArmorProficiencyGroup.Light: return 6;
                case ArmorProficiencyGroup.Medium: return 3;
                case ArmorProficiencyGroup.Heavy: return 1;
                default: return 6; // fallback sensato
            }
        }

        // Mapeo lineal: 27 - 3*MaxDex (guardarraíles)
        public static int ComputeArmorClassPenaltyFromMaxDex(int maxDex)
        {
            int reduction = 27 - 3 * maxDex;
            if (reduction < 0) reduction = 0;
            if (reduction > 999) reduction = 999;
            return reduction;
        }

        // --- Utilidad: ¿es físico? ---
        public static bool IsPhysical(DamageValue dv)
        {
            return dv.Source is PhysicalDamage;
        }

        // Heurística por grupo (ya no necesaria si usas GetArmorMaxDex arriba, la dejo por si la llamas en otro sitio)
        public static int GuessDexMaxByArmorGroup(ItemEntityArmor armorEntity)
        {
            try
            {
                var pg = armorEntity != null && armorEntity.Blueprint != null
                    ? armorEntity.Blueprint.ProficiencyGroup.ToString()
                    : null;

                switch (pg)
                {
                    case "Light": return 6;
                    case "Medium": return 3;
                    case "Heavy": return 1;
                    default: return 6;
                }
            }
            catch { return 6; }
        }

        // Porcentaje universal para reducir AC mostrado: 27 - 3*MaxDex (cap 0..27)
        public static int ComputeUniversalAcReductionPercent(int maxDex)
        {
            int p = 27 - 3 * maxDex;
            if (p < 0) p = 0;
            if (p > 27) p = 27;
            return p;
        }
    }
}
