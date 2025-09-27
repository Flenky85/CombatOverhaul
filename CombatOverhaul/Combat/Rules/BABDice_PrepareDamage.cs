/*using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;

namespace CombatOverhaul.Combat.Rules
{
    internal sealed class BABDice_PrepareDamage :
        IGlobalRulebookHandler<RulePrepareDamage>,
        ISubscriber, IGlobalSubscriber
    {
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            var unit = evt?.Initiator;
            if (unit == null) return;

            int bab = unit.Stats?.BaseAttackBonus ?? 0;
            int steps = bab / 6;
            if (steps <= 0) return;
            if (steps > 3) steps = 3;

            foreach (var d in evt.DamageBundle)
            {
                if (d == null) continue;
                if (d.Type != DamageType.Physical) continue;
                if (d.Precision) continue; // no tocar daño de precisión

                var cur = d.Dice.ModifiedValue;
                var next = DiceSizeProgression.Promote(cur, steps);
                if (next.Rolls == cur.Rolls && next.Dice == cur.Dice) continue;

                d.Dice.Modify(next, source: null);
            }
        }

        public void OnEventDidTrigger(RulePrepareDamage evt) { }
    }
}*/
