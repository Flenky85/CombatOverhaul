using System;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.EntitySystem.Stats;
using UnityEngine; // por si usas Mathf; si no, puedes quitarlo

namespace CombatOverhaul.Combat.Rules
{
    /// Escala TODO el daño mágico (Energy, Force) que hace el iniciador,
    /// multiplicándolo por (1 + 0.30 * INT_mod).
    /// Un único punto de intercepción: RuleCalculateDamage (fase pre-cálculo).
    internal sealed class IntelligenceMagicDamageScaling :
        IGlobalRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IGlobalSubscriber
    {
        // Ajusta aquí la potencia por punto de bonificador de INT
        private const float PerIntMod = 0.30f;

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            try
            {
                if (evt?.Initiator == null) return;

                // Bonificador de INT del que hace el daño (iniciador)
                int intMod = evt.Initiator.Stats?.Intelligence?.Bonus ?? 0;
                if (intMod <= 0) return;

                float mult = 1f + PerIntMod * intMod;
                if (mult <= 1f) return;

                // Recorremos el bundle y multiplicamos SOLO Energy / Force
                foreach (var dmg in evt.ParentRule.DamageBundle)
                {
                    if (dmg == null) continue;

                    if (dmg.Type == DamageType.Energy || dmg.Type == DamageType.Force)
                    {
                        // Empujar el multiplicador vía EmpowerBonus para que afecte dados y bonus
                        // y se integre en el flujo normal del cálculo (críticos, mitades, etc.)
                        int extraPercent = (int)Math.Round((mult - 1f) * 100f);
                        dmg.BonusPercent += extraPercent;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[CombatOverhaul][INT-MagicScale] Error: " + ex);
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) { /* no-op */ }
    }
}
