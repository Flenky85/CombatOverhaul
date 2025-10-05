using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Patches.Attack
{
    [HarmonyPatch(typeof(RuleCalculateAttacksCount), "CalculatePenalizedAttacksCount")]
    internal static class RemoveBABIteratives
    {
        static bool Prefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }
}
