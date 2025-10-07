using CombatOverhaul.Calculators;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

[HarmonyPatch(typeof(RuleSkillCheck), nameof(RuleSkillCheck.IsSuccessRoll))]
internal static class Patch_SkillCheck
{
    static bool Prefix(RuleSkillCheck __instance, int d20, int successBonus, ref bool __result)
    {
        if (__instance == null) { __result = false; return false; }

        var unit = __instance.Initiator;
        if (unit == null || unit.CombatState == null || !unit.CombatState.IsInCombat)
            return true; 

        int A = __instance.TotalBonus + successBonus;
        int D = __instance.DifficultyClass;

        var res = OpposedRollCore.ResolveD20(A, D, d20);
        __result = res.Success; 
        return false;
    }
}
