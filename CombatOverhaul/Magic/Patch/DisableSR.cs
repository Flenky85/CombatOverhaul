using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Magic.Patch
{
    [HarmonyPatch(typeof(RuleSpellResistanceCheck))]
    internal static class DisableSR
    {
        [HarmonyPatch(nameof(RuleSpellResistanceCheck.HasSpellResistance), MethodType.Getter)]
        [HarmonyPrefix]
        static bool HasSpellResistance_Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }
}
