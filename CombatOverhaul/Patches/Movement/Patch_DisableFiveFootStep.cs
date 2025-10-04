using HarmonyLib;
using Kingmaker.Controllers.Combat;        // UnitCombatState
using Kingmaker.EntitySystem.Entities;     // UnitEntityData
using TurnBased.Controllers;               // TurnController
using UnityEngine;                         // Debug

namespace CombatOverhaul.Patches.Movement
{
    /// <summary>
    /// Fuerza a que el Five-Foot Step esté SIEMPRE desactivado.
    /// - Parchea el getter de UnitCombatState.TBMState.EnabledFiveFootStep
    /// - Parchea también TurnController.GetEnabledFiveFootStep(UnitEntityData) como red de seguridad
    /// </summary>
    internal static class Patch_DisableFiveFootStep
    {
        private const string TAG = "[CO][5FTSTEP] ";

        // 1) Bloquea la propiedad usada por UI/lógica del unit (más común)
        [HarmonyPatch(typeof(UnitCombatState.TBMState), "get_EnabledFiveFootStep")]
        private static class Patch_TBMState_get_EnabledFiveFootStep
        {
            [HarmonyPrefix]
            private static bool Prefix(ref bool __result)
            {
                __result = false;
                return false; // saltar original
            }
        }

        // 2) Red de seguridad: bloquea el método del TurnController
        //    (algunas rutas consultan directamente al controller)
        [HarmonyPatch(typeof(TurnController), nameof(TurnController.GetEnabledFiveFootStep))]
        private static class Patch_TurnController_GetEnabledFiveFootStep
        {
            [HarmonyPrefix]
            private static bool Prefix(UnitEntityData unit, ref bool __result)
            {
                __result = false;
                return false; // saltar original
            }
        }
    }
}
