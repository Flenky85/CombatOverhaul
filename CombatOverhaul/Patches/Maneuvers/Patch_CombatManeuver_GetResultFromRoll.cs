using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.RuleSystem.Rules;
using CombatOverhaul.Combat.Calculators;

namespace CombatOverhaul.Patches.Maneuvers
{
    /// Sustituye la comprobación de éxito de maniobras por TN derivado de p = (A/(A+D))*α + β
    /// (topes 5–95 % y pasos del 5 %), respetando críticos y casos especiales vanilla.
    [HarmonyPatch(typeof(RuleCombatManeuver), nameof(RuleCombatManeuver.GetResultFromRoll))]
    internal static class Patch_CombatManeuver_GetResultFromRoll
    {
        static bool Prefix(RuleCombatManeuver __instance, int d20, ref CombatManeuverResult __result)
        {
            // Si no hay tirada preparada, falla como vanilla
            if (__instance.InitiatorRoll == null)
            {
                __result = CombatManeuverResult.Fail;
                return false;
            }

            // Batallas tácticas: saltamos la lógica (como en ataque)
            if (TacticalCombatHelper.IsActive)
            {
                __result = CombatManeuverResult.Success;
                return false;
            }

            // Naturales (respetamos AlwaysChance como vanilla)
            if (d20 == 20)
            {
                __result = CombatManeuverResult.CriticalSuccess;
                return false;
            }
            if (d20 == 1 && !__instance.Initiator.State.Features.AlwaysChance)
            {
                __result = CombatManeuverResult.CriticalFail;
                return false;
            }

            // Caso especial: Weapon Snatcher usa su tirada de Trucos/Robar (Thievery)
            if (__instance.IsWeaponSnatcher)
            {
                __result = (__instance.ThieveryRoll != null && __instance.ThieveryRoll.Success)
                    ? CombatManeuverResult.Success
                    : CombatManeuverResult.Fail;
                return false;
            }

            // Datos ya calculados por OnTrigger antes de llamar a GetResultFromRoll
            int A = __instance.InitiatorCMB;
            int D = __instance.TargetCMD;

            // Resolver con nuestros parámetros (α=1.3, β=0.09, floor=5 %, ceil=95 %, step=5 %)
            // Usa un resolver específico para maniobras (mismo núcleo que ataques)
            var res = OpposedRollCore.ResolveD20(A, D, d20);
            //OpposedRollStore.Save(__instance, res);

            __result = res.Success ? CombatManeuverResult.Success : CombatManeuverResult.Fail;

            // Saltamos la lógica original
            return false;
        }
    }
}
