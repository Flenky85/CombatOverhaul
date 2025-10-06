using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Settings;
using UnityEngine; // Mathf

namespace CombatOverhaul.UI
{
    [HarmonyPatch(typeof(UICombatTexts), nameof(UICombatTexts.GetTbmCombatText))]
    internal static class Patch_UICombatTexts_GetTbmCombatText
    {
        static bool Prefix(ref string __result, string text, int roll, int dc)
        {
            if (!SettingsRoot.Game.TurnBased.EnableTurnBaseCombatText || roll <= 0)
                return true; // deja el original para casos no-TBM o inválidos

            var tnOverride = TbmCombatTextContext.OverrideTN;

            // Si NO hay TN propio: no añadimos overlay; devolvemos el texto base
            if (!tnOverride.HasValue)
            {
                __result = text;
                TbmCombatTextContext.Clear();
                return false; // NO llamar al original (evita que añada "roll vs dc")
            }

            int tn = Mathf.Clamp(tnOverride.Value, 2, 20);
            __result = string.Format("{0}   (<sprite name=\"DiceD20White\"> {1} vs {2})", text, roll, tn);

            TbmCombatTextContext.Clear();
            return false; // ya construido
        }
    }
}
