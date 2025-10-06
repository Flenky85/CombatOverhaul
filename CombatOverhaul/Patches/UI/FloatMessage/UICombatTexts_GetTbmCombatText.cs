using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Settings;
using UnityEngine;

namespace CombatOverhaul.Patches.UI.FloatMessage
{
    [HarmonyPatch(typeof(UICombatTexts), nameof(UICombatTexts.GetTbmCombatText))]
    internal static class UICombatTexts_GetTbmCombatText
    {
        static bool Prefix(ref string __result, string text, int roll)
        {
            if (!SettingsRoot.Game.TurnBased.EnableTurnBaseCombatText || roll <= 0)
                return true; 

            var tnOverride = TbmCombatTextContext.OverrideTN;

            
            if (!tnOverride.HasValue)
            {
                __result = text;
                TbmCombatTextContext.Clear();
                return false;
            }

            int tn = Mathf.Clamp(tnOverride.Value, 2, 20);
            __result = string.Format("{0}   (<sprite name=\"DiceD20White\"> {1} vs {2})", text, roll, tn);

            TbmCombatTextContext.Clear();
            return false;
        }
    }
}
