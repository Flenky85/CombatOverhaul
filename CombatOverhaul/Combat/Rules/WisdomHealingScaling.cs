using System;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage; // RuleHealDamage
using Kingmaker.EntitySystem.Stats;
using UnityEngine;

namespace CombatOverhaul.Combat.Rules
{
    /// +30% de curación por cada punto de bonificador de WIS del lanzador.
    /// Afecta a toda curación que pase por RuleHealDamage (hechizos, canalizar, varitas, etc.).
    internal sealed class WisdomHealingScaling :
        IGlobalRulebookHandler<RuleHealDamage>,
        ISubscriber, IGlobalSubscriber
    {
        private const float PerWisMod = 0.30f;

        public void OnEventAboutToTrigger(RuleHealDamage evt)
        {
            try
            {
                var healer = evt?.Initiator;
                if (healer == null) return;

                int wisMod = healer.Stats?.Wisdom?.Bonus ?? 0;
                if (wisMod <= 0) return;

                float multAdd = PerWisMod * wisMod; // p. ej. WIS +4 => +1.2 (120%)
                if (multAdd <= 0f) return;

                // Suma al multiplicador interno (num = 1 + ModifierBonus)
                // Podemos pasar evt.SourceFact como "source"; si es null, también vale.
                evt.AddModifierBonus(multAdd, evt.SourceFact);
            }
            catch (Exception ex)
            {
                Debug.LogError("[CombatOverhaul][WIS-HealScale] " + ex);
            }
        }

        public void OnEventDidTrigger(RuleHealDamage evt) { /* no-op */ }
    }
}
