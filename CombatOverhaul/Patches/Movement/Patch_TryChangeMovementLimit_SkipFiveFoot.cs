using System;
using HarmonyLib;
using TurnBased.Controllers;              // TurnController
using Kingmaker.EntitySystem.Entities;     // UnitEntityData

namespace CombatOverhaul.Patches.TBM
{
    /// <summary>
    /// Reemplaza la lógica de TurnController.TryChangeMovementLimit() para
    /// saltar siempre FiveFootStep en el ciclo:
    /// TwoActions <-> OneAction (sin paso intermedio).
    /// </summary>
    [HarmonyPatch(typeof(TurnController), "TryChangeMovementLimit")]
    internal static class Patch_TryChangeMovementLimit_SkipFiveFoot
    {
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

            // Estado actual y tamaño del enum
            var curr = __instance.CurrentMovementLimit;
            int length = Enum.GetValues(typeof(TurnController.MovementLimit)).Length;

            // Nuestro ciclo: saltar siempre FiveFootStep
            var next = curr;
            int guard = 0;

            do
            {
                next = (TurnController.MovementLimit)(((int)next + 1) % length);
                guard++;

                // seguridad para evitar bucles raros si el enum cambiase
                if (guard > length + 2)
                    break;

                // saltamos explícitamente el FiveFootStep
                if (next == TurnController.MovementLimit.FiveFootStep)
                    continue;

                // TwoActions siempre es válido
                if (next == TurnController.MovementLimit.TwoActions)
                    break;

                // OneAction solo si puede
                if (next == TurnController.MovementLimit.OneAction && canOneAction)
                    break;

                // si no cumple, sigue el ciclo
            }
            while (true);

            if (curr != next)
            {
                __instance.SetMovementLimit(next);

                // el original además marcaba m_NeedNewPredictions = true,
                // así que lo hacemos también (campo privado)
                var fNeed = AccessTools.Field(__instance.GetType(), "m_NeedNewPredictions");
                if (fNeed != null) fNeed.SetValue(__instance, true);

                __result = true;
            }
            else
            {
                __result = false;
            }

            return false; // ya hicimos todo, saltar original
        }
    }
}
