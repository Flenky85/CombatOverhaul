using HarmonyLib;
using Kingmaker.Controllers.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatOverhaul.Patches.Attack
{
    [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.IsFullAttackRestrictedBecauseOfMoveAction), MethodType.Getter)]
    public static class Patch_AllowFullAttackAfterMove
    {
        static bool Prefix(ref bool __result)
        {
            __result = false;
            return false; // Evita que se ejecute el getter original
        }
    }
}
