using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.Tooltip;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;

namespace CombatOverhaul.Armor.Patch.UI
{
    [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.GetBody))]
    internal static class RemoveMaxDex
    {
        private static readonly string MaxDexName =
            UIUtility.GetGlossaryEntryName(TooltipElement.MaxDexterity.ToString()) ?? string.Empty;

        static void Postfix(TooltipTemplateItem __instance, ref IEnumerable<ITooltipBrick> __result)
        {
            if (__result == null) return;

            ItemEntityArmor armor = __instance != null ? __instance.m_Item as ItemEntityArmor : null;
            if (armor == null) return;

            List<ITooltipBrick> bricks = __result is IList<ITooltipBrick> list ? new List<ITooltipBrick>(list) : new List<ITooltipBrick>(__result);
            if (bricks.Count == 0) return;

            var filtered = new List<ITooltipBrick>(bricks.Count);
            int i = 0;
            while (i < bricks.Count)
            {
                ITooltipBrick b = bricks[i];

                if (!(b is TooltipBrickIconValueStat ivs) || !string.Equals(ivs.m_Name ?? string.Empty, MaxDexName, StringComparison.Ordinal))
                {
                    filtered.Add(b);
                    i++;
                    continue;
                }

                i++;

                while (i < bricks.Count && bricks[i] is TooltipBrickText) i++;

                if (i < bricks.Count && bricks[i] is TooltipBrickSeparator) i++;
            }

            __result = filtered;
        }
    }
}
