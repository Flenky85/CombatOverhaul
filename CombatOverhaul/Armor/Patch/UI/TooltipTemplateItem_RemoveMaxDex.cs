using Kingmaker.UI.Tooltip;
using Owlcat.Runtime.UI.Tooltips;
using System.Collections.Generic;

namespace CombatOverhaul.Armor.Patch.UI
{
    internal static class TooltipTemplateItem_RemoveMaxDex
    {
        public static void RemoveMaxDexBrick(List<ITooltipBrick> bricks)
        {
            TooltipTemplateItem_BrickHelpers.RemoveIconStatWithTrailingByGlossaryKey(
                bricks,
                TooltipElement.MaxDexterity.ToString(),
                removeFollowingText: true,
                removeFollowingSeparator: true
            );
        }
    }
}
