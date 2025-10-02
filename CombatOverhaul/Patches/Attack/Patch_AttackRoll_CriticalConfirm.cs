using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem.Rules;
using CombatOverhaul.Combat.Opposed;

namespace CombatOverhaul.Patches.Attack
{
    /// Reevalúa la confirmación de crítico con OpposedRollCore:
    /// A = AttackBonus + CriticalConfirmationBonus
    /// D = TargetCriticalAC
    /// d20 = CriticalConfirmationD20
    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.OnTrigger))]
    internal static class Patch_AttackRoll_CriticalConfirm
    {
        static void Postfix(RuleAttackRoll __instance)
        {
            // 1) No tocamos el modo táctico vanilla
            if (TacticalCombatHelper.IsActive)
                return;

            // 2) Si no hay amenaza de crítico, nada que confirmar
            if (!__instance.IsCriticalRoll)
                return;

            // 3) Auto-confirm vanilla: respetar
            if (__instance.AutoCriticalConfirmation)
            {
                __instance.IsCriticalConfirmed = true;
                // El propio juego ya ajusta Result, no tocamos más
                return;
            }

            // 4) Aplica nuestro modelo a la confirmación
            //    A = AttackBonus + CriticalConfirmationBonus
            //    D = TargetCriticalAC
            //    d20 = CriticalConfirmationD20
            var res = OpposedRollCore.ResolveD20(
                __instance.AttackBonus + __instance.CriticalConfirmationBonus,
                __instance.TargetCriticalAC,
                __instance.CriticalConfirmationD20
            );

            bool confirmed = res.success;

            // 5) Fortificación puede anular el crítico confirmado
            if (__instance.TargetUseFortification && __instance.FortificationRoll > 0 && !__instance.FortificationOvercomed)
                confirmed = false;

            __instance.IsCriticalConfirmed = confirmed;

            // 6) Ajusta el resultado SOLO si seguimos en un estado de golpe normal/crítico.
            //    Evita pisar Parry / MirrorImage / Concealment u otros resultados especiales.
            if (__instance.IsHit && (__instance.Result == AttackResult.Hit || __instance.Result == AttackResult.CriticalHit))
            {
                __instance.Result = confirmed ? AttackResult.CriticalHit : AttackResult.Hit;
            }
        }
    }
}
