using CombatOverhaul.Roll.UI;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Roll.Patch
{
    [HarmonyPatch(typeof(RuleSavingThrow), nameof(RuleSavingThrow.IsSuccessRoll))]
    internal static class SavingThrow
    {
        static bool Prefix(RuleSavingThrow __instance, int d20, int successBonus, ref bool __result)
        {
            if (__instance == null) { __result = false; return false; }

            int A = __instance.StatValue + successBonus;
            int D = __instance.DifficultyClass;

            var res = OpposedRollCore.ResolveD20(A, D, d20);

            if (d20 == 20) { TbmCombatTextContext.Set(res.TN); __result = true; return false; }
            if (d20 == 1) { TbmCombatTextContext.Set(res.TN); __result = false; return false; }

            TbmCombatTextContext.Set(res.TN);
            __result = res.Success;
            return false;
        }
    }
}
