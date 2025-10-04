using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Combat;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.TurnBasedMode;
using Kingmaker.UI;
using Kingmaker.UnitLogic;
using System.Linq;
using TurnBased.Controllers;
using UnityEngine;

namespace CombatOverhaul.UI
{
    /// TBM: fuerza cursor de ataque/full-attack tanto si puedes atacar ya
    /// como si necesitas moverte primero (tras tu cambio de full attack after move).
    [HarmonyPatch(typeof(CursorController), nameof(CursorController.SetUnitCursor))]
    internal static class Patch_TBM_CursorFullAttackAfterMove
    {
        static void Postfix(CursorController __instance, bool isUnitHighlighted, UnitEntityData hoveredUnit)
        {
            if (!CombatController.IsInTurnBasedCombat()) return;
            if (Game.Instance.CurrentMode != GameModeType.Default) return;
            if (!isUnitHighlighted || hoveredUnit == null) return;

            var turn = Game.Instance.TurnBasedCombatController?.CurrentTurn;
            var selected = turn?.SelectedUnit;
            var tbm = selected?.CombatState?.TBM;
            if (turn == null || selected == null || tbm == null) return;

            if (turn.EnableDeliverTouch && turn.TouchAbility?.Data == turn.CurrentAbility) return;

            var actionState = tbm.ActionState;
            bool ranged = selected.IsRanged();
            bool outOfRange = actionState.IsOutOfRange;
            bool canAttackNow = selected.CanAttack(hoveredUnit);
            bool isForbidden = outOfRange || !canAttackNow;

            // --- 1) SIN MOVER: ¿hay múltiples ataques que alcanzan?
            if (canAttackNow)
            {
                var countsNow = CursorController.CountReachingAttacks(hoveredUnit);
                bool multiNow = countsNow.Length > 0 &&
                                (countsNow[0] >= 2 || (countsNow.Length > 1 && countsNow[1] >= 2));

                if (multiNow)
                {
                    __instance.SetFullAttackCursor(countsNow, isForbidden: false, rangedAttack: ranged);
                    return;
                }

                __instance.SetCursor(ranged ? CursorRoot.CursorType.RangeAttackCursor
                                            : CursorRoot.CursorType.AttackCursor,
                                     icon: null, forbidden: false, outOfRange: false, text: null, angle: 0f);
                return;
            }

            // --- 2) TRAS MOVER: ¿algún grupo tendría ≥2 ataques que alcanzan?
            var countsAfterMove = CursorController.CountReachingAttacks(hoveredUnit);
            bool multiAfter = countsAfterMove.Length > 0 &&
                              (countsAfterMove[0] >= 2 || (countsAfterMove.Length > 1 && countsAfterMove[1] >= 2));

            if (multiAfter)
            {
                __instance.SetFullAttackCursor(countsAfterMove, isForbidden, ranged);
                return;
            }

            // Si tras mover solo habría 1 ataque, deja el cursor vanilla (no forzamos “×1”).
        }
    }
}
