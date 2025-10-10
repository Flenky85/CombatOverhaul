using System;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using UnityEngine;

namespace CombatOverhaul.Bus
{
    internal sealed class IntelligenceHealingScaling :
        IGlobalRulebookHandler<RuleHealDamage>,
        ISubscriber, IGlobalSubscriber
    {
        private const float PerMod = 0.05f;

        public void OnEventAboutToTrigger(RuleHealDamage evt)
        {
            if (evt == null) return;

            try
            {
                var healer = evt.Initiator;
                if (healer == null) return;

                int intMod = healer.Stats?.Intelligence?.Bonus ?? 0;
                if (intMod <= 0) return;

                float add = PerMod * intMod; 
                if (add <= 0f) return;

                evt.AddModifierBonus(add, evt.SourceFact);
            }
            catch (Exception ex)
            {

                Debug.LogError($"[INT→Heal] {ex}");

            }
        }

        public void OnEventDidTrigger(RuleHealDamage evt) { /* no-op */ }
    }
}
