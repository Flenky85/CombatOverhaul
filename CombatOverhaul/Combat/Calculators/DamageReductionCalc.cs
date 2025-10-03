/*using System;
using Kingmaker.RuleSystem.Rules.Damage;
using UnityEngine;

namespace CombatOverhaul.Combat.Calculators
{
    /// Utilidades puras para aplicar reducciones porcentuales de daño.
    internal static class DamageReductionCalc
    {
        internal sealed class Outcome
        {
            public DamageValue NewValue; // DamageValue recalculado
            public float Factor;         // multiplicador aplicado (0..1)
            public int OriginalFinal;    // dv.FinalValue original
            public int TargetFinal;      // FinalValue tras factor
        }

        /// Aplica una reducción porcentual (0..1) al DamageValue.
        /// - reduction = 0.40 -> factor 0.60
        /// - maxReduction limita la reducción máxima (por defecto 100%)
        public static Outcome ApplyPercent(in DamageValue dv, float reduction, float maxReduction = 1.0f)
        {
            // ¡OJO!: DamageValue es struct, nunca es null

            int finalNow = dv.FinalValue;
            if (finalNow <= 0)
            {
                return new Outcome
                {
                    NewValue = dv,
                    Factor = 1f,
                    OriginalFinal = finalNow,
                    TargetFinal = finalNow
                };
            }

            float clampedReduction = Mathf.Clamp01(Mathf.Min(reduction, maxReduction));
            float factor = 1f - clampedReduction;

            int targetFinal = Mathf.RoundToInt(finalNow * factor);

            // Mantener la semántica de Reduction (Valor con Reducción)
            int newVWR = Math.Max(0, targetFinal + dv.Reduction);

            return new Outcome
            {
                NewValue = new DamageValue(dv.Source, newVWR, dv.RollAndBonusValue, dv.RollResult, dv.TacticalCombatDRModifier),
                Factor = factor,
                OriginalFinal = finalNow,
                TargetFinal = targetFinal
            };
        }
    }
}*/
