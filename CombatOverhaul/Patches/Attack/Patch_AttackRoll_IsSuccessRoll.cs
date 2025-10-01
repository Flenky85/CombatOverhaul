using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem.Rules;
using CombatOverhaul.Combat.Opposed;

namespace CombatOverhaul.Patches.Attack
{
    /// Sustituye la comprobación de éxito por TN derivado de p = (A/(A+D))*α + β (topes y pasos del 5 %).
    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.IsSuccessRoll))]
    internal static class Patch_AttackRoll_IsSuccessRoll
    {
        static bool Prefix(RuleAttackRoll __instance, int d20, ref bool __result)
        {
            // Táctico vanilla
            if (TacticalCombatHelper.IsActive)
            {
                __result = true;
                return false;
            }

            // Forzados vanilla
            if (__instance.AutoHit) { __result = true; return false; }
            if (__instance.AutoMiss) { __result = false; return false; }

            // Naturales (respetamos AlwaysChance como vanilla)
            if (d20 == 20) { __result = true; return false; }
            if (d20 == 1 && !__instance.Initiator.State.Features.AlwaysChance)
            {
                __result = false;
                return false;
            }

            // Datos ya calculados por OnTrigger antes de llamar a IsSuccessRoll
            int A = __instance.AttackBonus;
            int D = __instance.TargetAC;

            // Resuelve con nuestros parámetros (α=1.3, β=0.09, floor=5 %, ceil=95 %, step=5 %)
            var res = OpposedRollCore.ResolveForAttack(A, D, d20);
            OpposedRollStore.Save(__instance, res);
            __result = res.success;

            // Saltamos la lógica original
            return false;
        }
    }
}
