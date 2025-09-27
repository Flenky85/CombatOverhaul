using System;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage; // RuleHealDamage
using Kingmaker.EntitySystem.Stats;
using UnityEngine;

namespace CombatOverhaul.Combat.Rules
{
    /// +30% de curación por cada punto de bonificador de INT del lanzador.
    /// Afecta a toda curación que pase por RuleHealDamage (hechizos, canalizar, varitas, etc.).
    internal sealed class IntelligenceHealingScaling :
        IGlobalRulebookHandler<RuleHealDamage>,
        ISubscriber, IGlobalSubscriber
    {
        private const float PerMod = 0.30f;

        public void OnEventAboutToTrigger(RuleHealDamage evt)
        {
            try
            {
                var healer = evt?.Initiator;
                if (healer == null) return;

                int intMod = healer.Stats?.Intelligence?.Bonus ?? 0;
                if (intMod <= 0) return;

                float multAdd = PerMod * intMod; // p. ej. INT +4 => +1.2 (120%)
                if (multAdd <= 0f) return;

                // Suma al multiplicador interno (num = 1 + ModifierBonus)
                // Podemos pasar evt.SourceFact como "source"; si es null, también vale.
                evt.AddModifierBonus(multAdd, evt.SourceFact);
            }
            catch (Exception ex)
            {
                Debug.LogError("[CombatOverhaul][INT-HealScale] " + ex);
            }
        }

        public void OnEventDidTrigger(RuleHealDamage evt) { /* no-op */ }
    }
}
