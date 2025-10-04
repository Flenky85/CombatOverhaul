using System;
using System.Reflection;
using HarmonyLib;
using TurnBased.Controllers;              // TurnController
using Kingmaker.EntitySystem.Entities;    // UnitEntityData

namespace CombatOverhaul.Patches.TBM
{
    /// <summary>
    /// Reemplaza la lógica de TurnController.TryChangeMovementLimit()
    /// para saltar FiveFootStep en el ciclo TwoActions <-> OneAction.
    /// </summary>
    [HarmonyPatch(typeof(TurnController), "TryChangeMovementLimit")]
    internal static class Patch_TryChangeMovementLimit_SkipFiveFoot
    {
        // Cache: tamaño del enum (evita Enum.GetValues cada vez)
        private static readonly int s_movementLimitCount =
            (int)Enum.GetValues(typeof(TurnController.MovementLimit)).Length;

        // Cache: acceso rápido al campo privado m_NeedNewPredictions sin reflexión por llamada
        private static readonly AccessTools.FieldRef<TurnController, bool> s_needNewPredictionsRef;
        // Fallback por si FieldRef fallara (mod obfuscado/cambios de versión)
        private static readonly FieldInfo s_needNewPredictionsFI;

        static Patch_TryChangeMovementLimit_SkipFiveFoot()
        {
            try
            {
                s_needNewPredictionsRef =
                    AccessTools.FieldRefAccess<TurnController, bool>("m_NeedNewPredictions");
            }
            catch
            {
                // Fallback: cachear FieldInfo (se usa solo si el FieldRef no se pudo crear)
                s_needNewPredictionsFI =
                    AccessTools.Field(typeof(TurnController), "m_NeedNewPredictions");
            }
        }

        [HarmonyPrefix]
        private static bool Prefix(TurnController __instance, ref bool __result)
        {
            // Si está moviéndose, el original devolvía false
            if (__instance.IsMoving)
            {
                __result = false;
                return false; // no ejecutar el original
            }

            // El original calcula si puede usar 5ft y si puede usar 1 acción
            UnitEntityData unit = __instance.Mount ?? __instance.Rider;

            bool canFiveFoot = __instance.HasFiveFootStep(unit);
            bool canOneAction = __instance.HasNormalMovement(unit)
                                && unit.CombatState.Cooldown.MoveAction < 3f
                                && !unit.UsedStandardAction();

            // Estado actual
            var curr = __instance.CurrentMovementLimit;
            var next = curr;

            // Nuestro ciclo: saltar siempre FiveFootStep
            // Usamos un guard pequeño por seguridad (enum cambiante).
            int guard = 0;
            while (true)
            {
                next = (TurnController.MovementLimit)(((int)next + 1) % s_movementLimitCount);
                guard++;
                if (guard > s_movementLimitCount + 2) break;

                // saltamos explícitamente el FiveFootStep
                if (next == TurnController.MovementLimit.FiveFootStep) continue;

                // TwoActions siempre es válido
                if (next == TurnController.MovementLimit.TwoActions) break;

                // OneAction solo si puede
                if (next == TurnController.MovementLimit.OneAction && canOneAction) break;

                // si no cumple, sigue el ciclo
            }

            if (curr != next)
            {
                __instance.SetMovementLimit(next);
                SetNeedNewPredictions(__instance, true);
                __result = true;
            }
            else
            {
                __result = false;
            }

            return false; // ya hicimos todo, saltar original
        }

        private static void SetNeedNewPredictions(TurnController inst, bool value)
        {
            // Usamos el FieldRef si está disponible (cero reflexión por llamada)
            if (s_needNewPredictionsRef != null)
            {
                s_needNewPredictionsRef(inst) = value;
                return;
            }

            // Fallback seguro (una reflexión ya cacheada en FieldInfo)
            s_needNewPredictionsFI?.SetValue(inst, value);
        }
    }
}
