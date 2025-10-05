using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using CombatOverhaul.Calculators;

namespace CombatOverhaul.Patches.Roll
{
    [HarmonyPatch(typeof(RuleSavingThrow), nameof(RuleSavingThrow.IsSuccessRoll))]
    internal static class SavingThrow
    {
        static bool Prefix(RuleSavingThrow __instance, int d20, int successBonus, ref bool __result)
        {
            if (d20 == 20) { __result = true; return false; }
            if (d20 == 1) { __result = false; return false; }

            int A = __instance.StatValue + successBonus;
            int D = __instance.DifficultyClass;

            var res = OpposedRollCore.ResolveD20(A, D, d20);
            __result = res.Success;
            return false; 
        }
    }
}
