using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Owlcat.Runtime.UI.Tooltips;

namespace CombatOverhaul.Armor.Patch.UI
{
    [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.GetBody))]
    [HarmonyPriority(Priority.Last)]
    internal static class TooltipTemplateItem_Postfix
    {
        static void Postfix(TooltipTemplateItem __instance, ref IEnumerable<ITooltipBrick> __result)
        {
            if (__result == null) return;

            var bricks = __result as List<ITooltipBrick> ?? new List<ITooltipBrick>(__result);

            TooltipTemplateItem_RemoveMaxDex.RemoveMaxDexBrick(bricks);

            if (__instance?.m_Item is ItemEntityArmor armor)
            {
                TooltipTemplateItem_AddArmorBricks.AddArmorBricks(armor, bricks);
            }

            __result = bricks;
        }
    }
}
