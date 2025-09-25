// File: Patch_AddArmorDRBrick.cs
using HarmonyLib;
using Kingmaker.Items; // ItemEntityArmor
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.MVVM._VM.Tooltip.Utils;
using Kingmaker.UI.Tooltip;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kingmaker.Blueprints.Items.Armors;

namespace CombatOverhaul.UI
{
    [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.GetBody))]
    static class Patch_AddArmorDRBrick
    {
        private static readonly FieldInfo NameField =
            AccessTools.Field(typeof(TooltipBrickIconValueStat), "m_Name");
        private static readonly FieldInfo TipField =
            AccessTools.Field(typeof(TooltipBrickIconValueStat), "m_Tooltip");

        private static readonly string[] GlossaryKeyCandidateFields = new[]
        { "m_GlossaryKey", "m_Key", "m_Entry", "m_GlossaryEntry", "m_Name" };

        static void Postfix(TooltipTemplateItem __instance, TooltipTemplateType type, ref IEnumerable<ITooltipBrick> __result)
        {
            if (__result == null) return;

            var armor = __instance.m_Item as ItemEntityArmor;
            if (armor == null) return;

            int dr = ComputeArmorDR(armor);
            if (dr <= 0) return;

            var bricks = __result.ToList();

            // 1) Brick DR
            var drBrick = new TooltipBrickIconValueStat(
                name: "Damage Reduction",
                value: $"{dr}%",
                icon: null,
                type: TooltipIconValueStatType.Normal,
                tooltip: null);

            // 2) Separador SMALL
            var sep1 = new TooltipBrickSeparator(TooltipBrickElementType.Small);

            // 3) Brick IconValueStat "Armor class penalty" con valor calculado
            int armorClassPenalty = ComputeArmorClassPenalty(armor);
            var penaltyBrick = new TooltipBrickIconValueStat(
                name: "Armor class penalty",
                value: $"{armorClassPenalty}%",
                icon: null,
                type: TooltipIconValueStatType.Normal,
                tooltip: null);

            // 4) Separador SMALL
            var sep2 = new TooltipBrickSeparator(TooltipBrickElementType.Small);

            // Posición: antes de ArmorCheckPenalty
            int insertIdx = FindBrickIndexByGlossaryKey(bricks, "ArmorCheckPenalty");
            if (insertIdx < 0)
            {
                string acpLabel = UIUtility.GetGlossaryEntryName(TooltipElement.ArmorCheckPenalty.ToString());
                insertIdx = bricks.FindIndex(b =>
                {
                    var s = b as TooltipBrickIconValueStat;
                    if (s == null || NameField == null) return false;
                    var name = NameField.GetValue(s) as string ?? "";
                    return name == acpLabel
                        || name.IndexOf("Penalizador", StringComparison.OrdinalIgnoreCase) >= 0
                        || name.IndexOf("Armor Check", StringComparison.OrdinalIgnoreCase) >= 0;
                });
            }

            if (insertIdx >= 0)
            {
                bricks.Insert(insertIdx, drBrick);
                bricks.Insert(++insertIdx, sep1);
                bricks.Insert(++insertIdx, penaltyBrick);
                bricks.Insert(++insertIdx, sep2);
            }
            else
            {
                bricks.Add(drBrick);
                bricks.Add(sep1);
                bricks.Add(penaltyBrick);
                bricks.Add(sep2);
            }

            __result = bricks;
        }

        private static int FindBrickIndexByGlossaryKey(List<ITooltipBrick> bricks, string key)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                var s = bricks[i] as TooltipBrickIconValueStat;
                if (s == null || TipField == null) continue;

                var tip = TipField.GetValue(s) as TooltipBaseTemplate;
                if (tip == null) continue;

                var t = tip.GetType();
                foreach (var fname in GlossaryKeyCandidateFields)
                {
                    var f = AccessTools.Field(t, fname);
                    if (f == null || f.FieldType != typeof(string)) continue;
                    var k = f.GetValue(tip) as string;
                    if (string.Equals(k, key, StringComparison.Ordinal)) return i;
                }
            }
            return -1;
        }

        // -----------------------
        // Cálculos
        // -----------------------

        private static int ComputeArmorDR(ItemEntityArmor armor)
        {
            var bpArmor = armor?.Blueprint as BlueprintItemArmor;
            int baseReal = bpArmor?.Type?.ArmorBonus ?? 0;
            return Math.Max(0, baseReal * 5);
        }

        // Mapea MaxDex → reducción AC según tu tabla (lineal: 27 - 3*MaxDex)
        private static int ComputeArmorClassPenalty(ItemEntityArmor armor)
        {
            int maxDex = GetArmorMaxDex(armor);
            // Tabla: 8→3, 7→6, ..., 0→27  =>  27 - 3*Dex
            int reduction = 27 - 3 * maxDex;
            if (reduction < 0) reduction = 0;
            if (reduction > 999) reduction = 999; // guardarraíl
            return reduction;
        }

        // Intenta leer el MaxDex de la armadura desde el blueprint/type
        private static int GetArmorMaxDex(ItemEntityArmor armor)
        {
            // Ruta más probable
            var bpArmor = armor?.Blueprint as BlueprintItemArmor;
            var type = bpArmor?.Type;
            if (type == null) return 0;

            // Propiedades/comunes entre builds
            // Prioriza propiedades públicas, luego campos, con varios alias conocidos.
            var intNames = new[] { "MaxDexterityBonus", "MaxDexterity", "MaxDexBonus", "MaxDex" };

            // 1) Propiedades
            foreach (var name in intNames)
            {
                var p = type.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (p != null && p.PropertyType == typeof(int))
                {
                    try { return (int)p.GetValue(type); } catch { }
                }
            }

            // 2) Campos
            foreach (var name in intNames)
            {
                var f = type.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (f != null && f.FieldType == typeof(int))
                {
                    try { return (int)f.GetValue(type); } catch { }
                }
            }

            // 3) Último recurso: escanea cualquier int con nombre que contenga "dex" y "max"
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
    }
}
