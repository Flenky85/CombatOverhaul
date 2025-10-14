using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Roll.Patch
{
    [HarmonyPatch(typeof(RuleCombatManeuver), nameof(RuleCombatManeuver.GetResultFromRoll))]
    internal static class Maneuver
    {
        private static bool Prefix(RuleCombatManeuver __instance, int d20, ref CombatManeuverResult __result)
        {
            if (__instance == null)
            {
                __result = CombatManeuverResult.Fail;
                return false;
            }

            if (__instance.InitiatorRoll == null)
            {
                __result = CombatManeuverResult.Fail;
                return false;
            }

            if (TacticalCombatHelper.IsActive)
            {
                __result = CombatManeuverResult.Success;
                return false;
            }

            var natural = TryResolveNatural(__instance, d20);
            if (natural.HasValue)
            {
                __result = natural.Value;
                return false;
            }

            if (__instance.IsWeaponSnatcher)
            {
                __result = __instance.ThieveryRoll != null && __instance.ThieveryRoll.Success
                    ? CombatManeuverResult.Success
                    : CombatManeuverResult.Fail;
                return false;
            }

            int A = __instance.InitiatorCMB;
            int D = __instance.TargetCMD;

            var res = OpposedRollCore.ResolveD20(A, D, d20);

            __result = res.Success ? CombatManeuverResult.Success : CombatManeuverResult.Fail;
            return false;
        }

        private static CombatManeuverResult? TryResolveNatural(RuleCombatManeuver evt, int d20)
        {
            if (d20 == 20) return CombatManeuverResult.CriticalSuccess;

            bool alwaysChance = evt?.Initiator?.State?.Features?.AlwaysChance ?? false;
            if (d20 == 1 && !alwaysChance)
                return CombatManeuverResult.CriticalFail;

            return null;
        }
    }
}
