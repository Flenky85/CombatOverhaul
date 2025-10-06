using System;
using CombatOverhaul.Calculators;
using CombatOverhaul.UI;
using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Patches.Roll
{
    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.IsSuccessRoll))]
    internal static class AttackRoll
    {
        private static bool Prefix(RuleAttackRoll __instance, int d20, ref bool __result)
        {
            if (__instance == null)
            {
                __result = false;
                return false;
            }

            if (TacticalCombatHelper.IsActive)
                return true;

            if (__instance.AutoHit)
            {
                __result = true;
                return false;
            }

            if (__instance.AutoMiss)
            {
                __result = false;
                return false;
            }

            int A = __instance.AttackBonus;
            int D = __instance.TargetAC;

            var res = OpposedRollCore.ResolveD20(A, D, d20);

            if (d20 == 20)
            {
                TbmCombatTextContext.Set(res.TN);
                __result = true;
                return false;
            }

            if (d20 == 1 && !__instance.Initiator.State.Features.AlwaysChance)
            {
                TbmCombatTextContext.Set(res.TN);
                __result = false;
                return false;
            }

            TbmCombatTextContext.Set(res.TN);
            __result = res.Success;
            return false;
        }
    }
}
