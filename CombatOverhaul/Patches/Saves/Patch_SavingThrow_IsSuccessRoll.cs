using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using CombatOverhaul.Combat.Opposed;

namespace CombatOverhaul.Patches.Saves
{
    /// Usa TN derivado de p=(A/(A+D))*α+β para las salvaciones.
    [HarmonyPatch(typeof(RuleSavingThrow), nameof(RuleSavingThrow.IsSuccessRoll))]
    internal static class Patch_SavingThrow_IsSuccessRoll
    {
        static bool Prefix(RuleSavingThrow __instance, int d20, int successBonus, ref bool __result)
        {
            // Naturales como vanilla
            if (d20 == 20) { __result = true; return false; }
            if (d20 == 1) { __result = false; return false; }

            // A = StatValue + successBonus | D = DC
            int A = __instance.StatValue + successBonus;
            int D = __instance.DifficultyClass;

            var res = OpposedRollCore.ResolveD20(A, D, d20);
            __result = res.success;
            return false; // saltar lógica original
        }
    }
}
