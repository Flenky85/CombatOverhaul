using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Roll.Patch
{
    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.OnTrigger))]
    internal static class CriticalConfirm
    {
        static void Postfix(RuleAttackRoll __instance)
        {
            if (__instance == null) return;
            if (TacticalCombatHelper.IsActive) return;
            if (!__instance.IsCriticalRoll) return;

            if (__instance.AutoCriticalConfirmation)
            {
                __instance.IsCriticalConfirmed = true;
                if (__instance.IsHit && (__instance.Result == AttackResult.Hit || __instance.Result == AttackResult.CriticalHit))
                    __instance.Result = AttackResult.CriticalHit;
                return;
            }

            var opposed = OpposedRollCore.ResolveD20(
                __instance.AttackBonus + __instance.CriticalConfirmationBonus,
                __instance.TargetCriticalAC,
                __instance.CriticalConfirmationD20
            );

            bool fortificationBlocks = __instance.TargetUseFortification
                                       && __instance.FortificationRoll > 0
                                       && !__instance.FortificationOvercomed;

            bool confirmed = opposed.Success && !fortificationBlocks;
            __instance.IsCriticalConfirmed = confirmed;

            if (__instance.IsHit && (__instance.Result == AttackResult.Hit || __instance.Result == AttackResult.CriticalHit))
                __instance.Result = confirmed ? AttackResult.CriticalHit : AttackResult.Hit;
        }
    }
}
