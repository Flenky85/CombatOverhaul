using CombatOverhaul.Calculators;
using CombatOverhaul.UI;
using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Patches.Attack
{
    /// Sustituye la comprobación de éxito por TN derivado de p = (A/(A+D))*α + β (topes y pasos del 5 %).
    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.IsSuccessRoll))]
    internal static class AttackRoll
    {
        static bool Prefix(RuleAttackRoll __instance, int d20, ref bool __result)
        {
            // Táctico vanilla
            if (TacticalCombatHelper.IsActive)
            {
                __result = true;
                return false;
            }

            int A = __instance.AttackBonus;
            int D = __instance.TargetAC;

            // Naturales/forzados: deja contexto también
            if (__instance.AutoHit)
            {
                var r = OpposedRollCore.ResolveD20(A, D, d20);
                TbmCombatTextContext.Set(r.TN, (int)System.Math.Round(r.P5 * 100f));
                __result = true;
                return false;
            }
            if (__instance.AutoMiss)
            {
                var r = OpposedRollCore.ResolveD20(A, D, d20);
                TbmCombatTextContext.Set(r.TN, (int)System.Math.Round(r.P5 * 100f));
                __result = false;
                return false;
            }

            // Naturales (respetamos AlwaysChance como vanilla)
            if (d20 == 20) { __result = true; return false; }
            if (d20 == 1 && !__instance.Initiator.State.Features.AlwaysChance)
            {
                __result = false;
                return false;
            }


            // Resuelve con nuestros parámetros (α=1.3, β=0.09, floor=5 %, ceil=95 %, step=5 %)
            var res = OpposedRollCore.ResolveD20(A, D, d20);
            TbmCombatTextContext.Set(res.TN, (int)System.Math.Round(res.P5 * 100f));
            //OpposedRollStore.Save(__instance, res);
            __result = res.Success;

            // Saltamos la lógica original
            return false;
        }
    }
}
