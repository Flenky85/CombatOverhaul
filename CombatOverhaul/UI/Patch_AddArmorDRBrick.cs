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
using CombatOverhaul.Calculators; // ArmorCalculator

namespace CombatOverhaul.UI
{
    [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.GetBody))]
    static class Patch_AddArmorDRBrick
    {
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
                value: dr + "%", // baseReal * 5
                icon: null,
                type: TooltipIconValueStatType.Normal,
                tooltip: null);

            // 2) Separador SMALL
            var sep1 = new TooltipBrickSeparator(TooltipBrickElementType.Small);

            // 3) Brick IconValueStat "Armor class penalty"
            int maxDex = ArmorCalculator.GetArmorMaxDex(armor);
            int armorClassPenalty = ArmorCalculator.ComputeAcReductionPercentFromMaxDex(maxDex);
            var penaltyBrick = new TooltipBrickIconValueStat(
                name: "Armor class penalty",
                value: armorClassPenalty + "%", // 27 - 3*MaxDex (clamp 0..27)
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
                    if (s == null) return false;
                    var name = s.m_Name ?? "";
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

        // Sin reflexión: comparamos contra el nombre “bonito” que devuelve el glosario
        private static int FindBrickIndexByGlossaryKey(List<ITooltipBrick> bricks, string key)
        {
            var expected = UIUtility.GetGlossaryEntryName(key);
            for (int i = 0; i < bricks.Count; i++)
            {
                if (bricks[i] is TooltipBrickIconValueStat s)
                {
                    var name = s.m_Name ?? "";
                    if (string.Equals(name, expected, StringComparison.Ordinal)) return i;

                    // Fallbacks ligeros por localización o variaciones menores
                    if (name.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0) return i;
                }
            }
            return -1;
        }
    }
}
