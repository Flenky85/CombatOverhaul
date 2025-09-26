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
using CombatOverhaul.Combat.Calculators; // ArmorCalculator

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

            int dr = ArmorCalculator.ComputeArmorDrDisplayPercent(armor);
            if (dr <= 0) return;

            var bricks = __result.ToList();

            // 1) Brick DR
            var drBrick = new TooltipBrickIconValueStat(
                name: "Damage Reduction",
                value: dr.ToString() + "%",
                icon: null,
                type: TooltipIconValueStatType.Normal,
                tooltip: null);

            // 2) Separador SMALL
            var sep1 = new TooltipBrickSeparator(TooltipBrickElementType.Small);

            // 3) Brick IconValueStat "Armor class penalty"
            int armorClassPenalty = ArmorCalculator.ComputeArmorClassPenaltyFromMaxDex(ArmorCalculator.GetArmorMaxDex(armor));
            var penaltyBrick = new TooltipBrickIconValueStat(
                name: "Armor class penalty",
                value: armorClassPenalty.ToString() + "%",
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
    }
}
