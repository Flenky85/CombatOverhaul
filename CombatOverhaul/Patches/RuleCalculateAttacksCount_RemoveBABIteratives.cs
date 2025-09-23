using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Patches
{
    [HarmonyPatch(typeof(RuleCalculateAttacksCount), "CalculatePenalizedAttacksCount")]
    internal static class RuleCalculateAttacksCount_RemoveBABIteratives
    {
        static bool Prefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }
}
