/*using System;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using UnityEngine;

namespace CombatOverhaul.Combat.Rules
{
    /// +1 paso de dado por cada 6 BAB (máx +3) sobre el dado base del arma.
    /// Se aplica aquí para que la UI muestre el dado promocionado.
    internal sealed class BABDice_WeaponStats :
        IGlobalRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IGlobalSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            try
            {
                var unit = evt?.Initiator;
                if (unit == null) return;

                int bab = unit.Stats?.BaseAttackBonus ?? 0;
                int steps = bab / 6;
                if (steps <= 0) return;
                if (steps > 3) steps = 3;

                // Tomamos el dado base actual del arma (antes de tamaño) y lo promocionamos.
                var cur = evt.WeaponDamageDice.ModifiedValue;
                var promoted = DiceSizeProgression.Promote(cur, steps);
                if (promoted.Rolls == cur.Rolls && promoted.Dice == cur.Dice) return;

                evt.WeaponDamageDice.Modify(promoted, source: null);

#if DEBUG
                Debug.Log($"[CO][BAB→UI] BAB={bab} steps={steps} {cur.Rolls}d{(int)cur.Dice} → {promoted.Rolls}d{(int)promoted.Dice}");
#endif
            }
            catch (Exception ex)
            {
                Debug.LogError("[CO][BAB→UI] " + ex);
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) { }
    }
}
*/