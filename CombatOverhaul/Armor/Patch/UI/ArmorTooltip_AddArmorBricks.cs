using Kingmaker.Items;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;

namespace CombatOverhaul.Armor.Patch.UI
{
    internal static class ArmorTooltip_AddArmorBricks
    {
        private const string Label_DamageReduction = "Damage Reduction";
        private const string Label_ArmorClassPenalty = "Armor class penalty";

        public static void AddArmorBricks(ItemEntityArmor armor, List<ITooltipBrick> bricks)
        {
            if (armor == null || bricks == null) return;

            int drPercent = ArmorCalculator.ComputeArmorDrDisplayPercent(armor);
            if (drPercent <= 0) return;

            int maxDex = ArmorCalculator.GetArmorMaxDex(armor);
            int acPenaltyPct = ArmorCalculator.ComputeAcReductionPercentFromMaxDex(maxDex);

            var block = new List<ITooltipBrick>(4)
            {
                ArmorTooltip_BrickHelpers.Stat(Label_DamageReduction,   drPercent   + "%"),
                ArmorTooltip_BrickHelpers.SepSmall(),
                ArmorTooltip_BrickHelpers.Stat(Label_ArmorClassPenalty, acPenaltyPct + "%"),
                ArmorTooltip_BrickHelpers.SepSmall(),
            };

            int insertIdx = FindBrickIndexByGlossaryKey(bricks, "ArmorCheckPenalty");
            ArmorTooltip_BrickHelpers.InsertBlockOrAppend(bricks, insertIdx, block);
        }

        private static int FindBrickIndexByGlossaryKey(List<ITooltipBrick> bricks, string glossaryKey)
        {
            if (bricks == null || bricks.Count == 0 || string.IsNullOrEmpty(glossaryKey))
                return -1;

            string expected = UIUtility.GetGlossaryEntryName(glossaryKey) ?? string.Empty;

            for (int i = 0; i < bricks.Count; i++)
            {
                if (bricks[i] is TooltipBrickIconValueStat stat)
                {
                    string name = stat.m_Name ?? string.Empty;

                    // match exacto primero
                    if (!string.IsNullOrEmpty(expected) &&
                        string.Equals(name, expected, StringComparison.Ordinal))
                        return i;

                    // match contiene (por si cambia el idioma o formato)
                    if (!string.IsNullOrEmpty(expected) &&
                        name.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0)
                        return i;

                    // heurísticas multi-idioma
                    if (name.IndexOf("Armor Check", StringComparison.OrdinalIgnoreCase) >= 0) return i;
                    if (name.IndexOf("Penalizador", StringComparison.OrdinalIgnoreCase) >= 0) return i;
                }
            }
            return -1;
        }
    }
}
