using CombatOverhaul.Damage;
using CombatOverhaul.Utils;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using System;
using UnityEngine;

namespace CombatOverhaul.Damage.EventBus
{
    internal sealed class BABDice_WeaponStats :
        IGlobalRulebookHandler<RuleCalculateWeaponStats>,
        ISubscriber, IGlobalSubscriber
    {
        private const int StepPerDie = 4;
        private const int MaxSteps = 3;

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt == null) return;

            try
            {
                var unit = evt.Initiator;
                if (unit == null) return;

                int bab = unit.Stats?.BaseAttackBonus ?? 0;
                int steps = Mathf.Clamp(bab / StepPerDie, 0, MaxSteps);
                if (steps == 0) return;

                var cur = evt.WeaponDamageDice.ModifiedValue;
                var promoted = DiceSizeProgression.Promote(cur, steps);

                if (promoted.Rolls == cur.Rolls && promoted.Dice == cur.Dice)
                    return;

                evt.WeaponDamageDice.Modify(promoted, source: null);
            }
            catch (Exception ex)
            {

                Debug.LogError($"[BAB→Dice] {ex}");

            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt) { }
    }
}
