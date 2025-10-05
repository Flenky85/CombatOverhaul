using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Owlcat.Runtime.UI.Tooltips;
using CombatOverhaul.Calculators;

namespace CombatOverhaul.Patches.UI
{
    [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.GetBody))]
    internal static class AddArmorBrick
    {
        private const string Label_DamageReduction = "Damage Reduction";
        private const string Label_ArmorClassPenalty = "Armor class penalty";

        private static void Postfix(TooltipTemplateItem __instance, ref IEnumerable<ITooltipBrick> __result)
        {
            if (__result == null) return;

            if (!(__instance.m_Item is ItemEntityArmor armor)) return;

            int drPercent = ArmorCalculator.ComputeArmorDrDisplayPercent(armor);
            if (drPercent <= 0) return;

            int maxDex = ArmorCalculator.GetArmorMaxDex(armor);
            int acPenaltyPct = ArmorCalculator.ComputeAcReductionPercentFromMaxDex(maxDex);

            var bricks = __result as List<ITooltipBrick> ?? new List<ITooltipBrick>(__result);

            var drBrick = new TooltipBrickIconValueStat(
                name: Label_DamageReduction,
                value: drPercent + "%",
                icon: null,
                type: TooltipIconValueStatType.Normal,
                tooltip: null);

            var sep1 = new TooltipBrickSeparator(TooltipBrickElementType.Small);

            var penaltyBrick = new TooltipBrickIconValueStat(
                name: Label_ArmorClassPenalty,
                value: acPenaltyPct + "%",
                icon: null,
                type: TooltipIconValueStatType.Normal,
                tooltip: null);

            var sep2 = new TooltipBrickSeparator(TooltipBrickElementType.Small);

            int insertIdx = FindBrickIndexByGlossaryKey(bricks, "ArmorCheckPenalty");
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

        private static int FindBrickIndexByGlossaryKey(List<ITooltipBrick> bricks, string glossaryKey)
        {
            if (bricks == null || bricks.Count == 0 || string.IsNullOrEmpty(glossaryKey))
                return -1;

            string expected = UIUtility.GetGlossaryEntryName(glossaryKey);

            for (int i = 0; i < bricks.Count; i++)
            {
                if (!(bricks[i] is TooltipBrickIconValueStat stat)) continue;

                string name = stat.m_Name ?? string.Empty;

                if (string.Equals(name, expected, StringComparison.Ordinal))
                    return i;

                if (name.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0)
                    return i;

                if (name.IndexOf("Armor Check", StringComparison.OrdinalIgnoreCase) >= 0) return i;
                if (name.IndexOf("Penalizador", StringComparison.OrdinalIgnoreCase) >= 0) return i;
            }

            return -1;
        }
    }
}
