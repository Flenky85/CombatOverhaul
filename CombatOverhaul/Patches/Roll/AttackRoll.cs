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
            {
                __result = true;
                return false;
            }

            int A = __instance.AttackBonus;
            int D = __instance.TargetAC;

            if (__instance.AutoHit)
            {
                var r = OpposedRollCore.ResolveD20(A, D, d20);
                TbmCombatTextContext.Set(r.TN, (int)Math.Round(r.P5 * 100f));
                __result = true;
                return false;
            }

            if (__instance.AutoMiss)
            {
                var r = OpposedRollCore.ResolveD20(A, D, d20);
                TbmCombatTextContext.Set(r.TN, (int)Math.Round(r.P5 * 100f));
                __result = false;
                return false;
            }

            if (d20 == 20)
            {
                __result = true;
                return false;
            }

            if (d20 == 1 && !__instance.Initiator.State.Features.AlwaysChance)
            {
                __result = false;
                return false;
            }

            var res = OpposedRollCore.ResolveD20(A, D, d20);
            TbmCombatTextContext.Set(res.TN, (int)Math.Round(res.P5 * 100f));
            __result = res.Success;

            return false;
        }
    }
}
