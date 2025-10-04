using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Settings;
using CombatOverhaul.UI;

namespace CombatOverhaul.UI
{
    [HarmonyPatch(typeof(UICombatTexts), nameof(UICombatTexts.GetTbmCombatText))]
    internal static class Patch_UICombatTexts_GetTbmCombatText
    {
        static bool Prefix(ref string __result, string text, int roll, int dc)
        {
            if (!SettingsRoot.Game.TurnBased.EnableTurnBaseCombatText || roll <= 0) return true;

            int tn = TbmCombatTextContext.OverrideTN ?? dc;
            var pct = TbmCombatTextContext.OverridePct;

            __result = pct.HasValue
                ? $"{text}   (<sprite name=\"DiceD20White\"> {roll} vs {tn})"
                : $"{text}   (<sprite name=\"DiceD20White\"> {roll} vs {tn})";

            TbmCombatTextContext.Clear();
            return false;
        }
    }
}
