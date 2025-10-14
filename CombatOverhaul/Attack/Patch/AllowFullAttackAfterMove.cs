using HarmonyLib;
using Kingmaker.Controllers.Combat;

namespace CombatOverhaul.Attack.Patch
{
    [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.IsFullAttackRestrictedBecauseOfMoveAction), MethodType.Getter)]
    public static class AllowFullAttackAfterMove
    {
        static bool Prefix(ref bool __result)
        {
            __result = false;
            return false; 
        }
    }
}
