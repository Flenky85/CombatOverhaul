using System;
using System.Reflection;
using HarmonyLib;
using TurnBased.Controllers;
using Kingmaker.EntitySystem.Entities;

namespace CombatOverhaul.Movement.Patch
{
    [HarmonyPatch(typeof(TurnController), "TryChangeMovementLimit")]
    internal static class SkipFiveFootMove
    {
        private static readonly int s_movementLimitCount =
            Enum.GetValues(typeof(TurnController.MovementLimit)).Length;

        private static readonly AccessTools.FieldRef<TurnController, bool> s_needNewPredictionsRef;
        private static readonly FieldInfo s_needNewPredictionsFI;

        static SkipFiveFootMove()
        {
            try
            {
                s_needNewPredictionsRef = AccessTools.FieldRefAccess<TurnController, bool>("m_NeedNewPredictions");
            }
            catch
            {
                s_needNewPredictionsFI = AccessTools.Field(typeof(TurnController), "m_NeedNewPredictions");
            }
        }

        [HarmonyPrefix]
        private static bool Prefix(TurnController __instance, ref bool __result)
        {
            if (__instance == null)
            {
                __result = false;
                return false;
            }

            if (__instance.IsMoving)
            {
                __result = false;
                return false; 
            }

            UnitEntityData unit = __instance.Mount ?? __instance.Rider;
            if (unit == null)
            {
                __result = false;
                return false;
            }

            _ = __instance.HasFiveFootStep(unit);

            bool canOneAction =
                __instance.HasNormalMovement(unit) &&
                unit.CombatState.Cooldown.MoveAction < 3f &&
                !unit.UsedStandardAction();

            var curr = __instance.CurrentMovementLimit;
            var next = curr;

            int guard = 0;
            while (true)
            {
                next = (TurnController.MovementLimit)(((int)next + 1) % s_movementLimitCount);
                guard++;
                if (guard > s_movementLimitCount + 2) break; 

                if (next == TurnController.MovementLimit.FiveFootStep)
                    continue; 

                if (next == TurnController.MovementLimit.TwoActions)
                    break;

                if (next == TurnController.MovementLimit.OneAction && canOneAction)
                    break;
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

            return false;
        }

        private static void SetNeedNewPredictions(TurnController inst, bool value)
        {
            if (s_needNewPredictionsRef != null)
            {
                s_needNewPredictionsRef(inst) = value;
                return;
            }

            s_needNewPredictionsFI?.SetValue(inst, value);
        }
    }
}
