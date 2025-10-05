using System;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using UnityEngine;

namespace CombatOverhaul.Combat.Rules.Bus
{
    internal sealed class IntelligenceMagicDamageScaling :
        IGlobalRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IGlobalSubscriber
    {
        private const float PerIntMod = 0.10f;

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (evt == null) return;

            try
            {
                var initiator = evt.Initiator;
                if (initiator == null) return;

                int intMod = initiator.Stats?.Intelligence?.Bonus ?? 0;
                if (intMod <= 0) return;

                int addPercent = (int)Math.Round(PerIntMod * intMod * 100f);
                if (addPercent <= 0) return;

                var bundle = evt.ParentRule?.DamageBundle;
                if (bundle == null) return;

                foreach (var dmg in bundle)
                {
                    if (dmg == null) continue;

                    if (dmg.Type == DamageType.Energy || dmg.Type == DamageType.Force)
                    {
                        dmg.BonusPercent += addPercent;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[INT→DMG] {ex}");
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) { /* no-op */ }
    }
}
