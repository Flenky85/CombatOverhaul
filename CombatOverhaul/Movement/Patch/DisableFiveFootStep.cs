using HarmonyLib;
using Kingmaker.Controllers.Combat;
using TurnBased.Controllers;

namespace CombatOverhaul.Movement.Patch
{
    internal static class DisableFiveFootStep
    {

        [HarmonyPatch(typeof(UnitCombatState.TBMState), "get_EnabledFiveFootStep")]
        private static class Patch_TBMState_get_EnabledFiveFootStep
        {
            [HarmonyPrefix]
            private static bool Prefix(ref bool __result)
            {
                __result = false;
                return false; 
            }
        }

        [HarmonyPatch(typeof(TurnController), nameof(TurnController.GetEnabledFiveFootStep))]
        private static class Patch_TurnController_GetEnabledFiveFootStep
        {
            [HarmonyPrefix]
            private static bool Prefix(ref bool __result)
            {
                __result = false;
                return false; 
            }
        }
    }
}
