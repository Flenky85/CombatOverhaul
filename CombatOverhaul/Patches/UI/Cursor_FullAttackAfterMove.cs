using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.UI;
using Kingmaker.UnitLogic;
using TurnBased.Controllers;

namespace CombatOverhaul.Patches.UI
{
    [HarmonyPatch(typeof(CursorController), nameof(CursorController.SetUnitCursor))]
    internal static class Cursor_FullAttackAfterMove
    {
        static void Postfix(CursorController __instance, bool isUnitHighlighted, UnitEntityData hoveredUnit)
        {
            if (!CombatController.IsInTurnBasedCombat()) return;
            if (Game.Instance.CurrentMode != GameModeType.Default) return;
            if (!isUnitHighlighted || hoveredUnit == null) return;

            var turn = Game.Instance.TurnBasedCombatController?.CurrentTurn;
            var selected = turn?.SelectedUnit;
            var tbm = selected != null && selected.CombatState != null ? selected.CombatState.TBM : null;
            if (turn == null || selected == null || tbm == null) return;

            if (!selected.HasStandardAction()) return;

            if (turn.EnableDeliverTouch && turn.TouchAbility != null && turn.TouchAbility.Data == turn.CurrentAbility) return;

            bool ranged = selected.IsRanged();
            bool canAttackNow = selected.CanAttack(hoveredUnit);
            bool outOfRange = tbm.ActionState != null && tbm.ActionState.IsOutOfRange;
            bool isForbidden = outOfRange || !canAttackNow;

            if (canAttackNow)
            {
                int[] countsNow = CursorController.CountReachingAttacks(hoveredUnit);
                if (HasMulti(countsNow))
                {
                    __instance.SetFullAttackCursor(countsNow, isForbidden: false, rangedAttack: ranged);
                    return;
                }

                SetSimpleAttackCursor(__instance, ranged);
                return;
            }

            int[] countsAfterMove = CursorController.CountReachingAttacks(hoveredUnit);
            if (HasMulti(countsAfterMove))
            {
                __instance.SetFullAttackCursor(countsAfterMove, isForbidden, ranged);
                return;
            }
        }

        private static bool HasMulti(int[] counts)
        {
            if (counts == null || counts.Length == 0) return false;
            if (counts[0] >= 2) return true;
            return counts.Length > 1 && counts[1] >= 2;
        }

        private static void SetSimpleAttackCursor(CursorController cc, bool ranged)
        {
            cc.SetCursor(
                ranged ? CursorRoot.CursorType.RangeAttackCursor : CursorRoot.CursorType.AttackCursor,
                icon: null,
                forbidden: false,
                outOfRange: false,
                text: null,
                angle: 0f
            );
        }
    }
}
