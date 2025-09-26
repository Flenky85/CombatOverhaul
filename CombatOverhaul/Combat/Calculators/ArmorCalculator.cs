using System;
using System.Reflection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class ArmorCalculator
    {
        // --- Lectura base de la armadura ---
        public static int GetArmorBase(ItemEntityArmor armorItem)
        {
            try
            {
                var bp = armorItem != null ? armorItem.Blueprint as BlueprintItemArmor : null;
                if (bp == null) return 0;

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
        public static int ComputeArmorDrDisplayPercent(ItemEntityArmor armor) // ex: ComputeArmorDR
        {
            var bpArmor = armor != null ? armor.Blueprint as BlueprintItemArmor : null;
            int baseReal = bpArmor != null && bpArmor.Type != null ? bpArmor.Type.ArmorBonus : 0;
            return Math.Max(0, baseReal * 5);
        }

        // --- UI: MaxDex y penalización asociada ---
        public static int GetArmorMaxDex(ItemEntityArmor armor)
        {
            var bpArmor = armor != null ? armor.Blueprint as BlueprintItemArmor : null;
            var type = bpArmor != null ? bpArmor.Type : null;
            if (type == null) return 0;

            var intNames = new[] { "MaxDexterityBonus", "MaxDexterity", "MaxDexBonus", "MaxDex" };

            // Propiedades
            foreach (var name in intNames)
            {
                var p = type.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (p != null && p.PropertyType == typeof(int))
                {
                    try { return (int)p.GetValue(type); } catch { }
                }
            }

            // Campos
            foreach (var name in intNames)
            {
                var f = type.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (f != null && f.FieldType == typeof(int))
                {
                    try { return (int)f.GetValue(type); } catch { }
                }
            }

            // Heurística final
            var fields = type.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var f in fields)
            {
                if (f.FieldType == typeof(int))
                {
                    var n = f.Name.ToLowerInvariant();
                    if (n.Contains("dex") && n.Contains("max"))
                    {
                        try { return (int)f.GetValue(type); } catch { }
                    }
                }
            }

            return 0;
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
            // dv es struct; dv.Source puede ser null, el patrón 'is' ya devuelve false si lo es
            return dv.Source is PhysicalDamage;
        }
        // Heurística por grupo de armadura si no localizamos el MaxDex real
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
