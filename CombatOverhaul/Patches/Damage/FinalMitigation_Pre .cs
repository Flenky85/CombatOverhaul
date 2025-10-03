/*
using CombatOverhaul.Combat.Calculators; // ArmorCalculator, SRCalculator
using CombatOverhaul.Combat.Rules;       // ArmorDR_FactorStore
using HarmonyLib;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;              // DamageType
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatOverhaul.Patches.Damage
{
    /// Mitigación PRE-cálculo: restamos % al DamageBundle con BonusPercent negativo.
    /// - Físico (B/P/S): RD por armadura/markers
    /// - No físico      : SR (1% por punto)
    /// Además rellenamos factores para "(x0,xx)" en el log.
    internal sealed class FinalMitigation_Pre :
        IGlobalRulebookHandler<RuleCalculateDamage>, ISubscriber, IGlobalSubscriber
    {
        private const float MaxFinalReduction = 1.0f;

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            try
            {
                var target = evt?.Target;
                var targetDesc = target?.Descriptor;
                var bundle = evt?.ParentRule?.DamageBundle;
                if (targetDesc == null || bundle == null)
                    return;

                MechanicsContext ctx = evt.ParentRule?.Reason?.Context;

                var factors = new List<float>(); // no usamos Count ni indexador

                foreach (var comp in bundle)
                {
                    if (comp == null)
                    {
                        factors.Add(1f);
                        continue;
                    }

                    // Tipo del componente
                    bool isPhysical = comp.Type == DamageType.Physical;

                    float rd = 0f;
                    if (isPhysical)
                    {
                        // === ARMADURA (tu lógica) ===
                        float rdBase = 0f;

                        var armorSlot = target?.Body?.Armor;
                        var armorItem = (armorSlot != null && armorSlot.HasArmor) ? armorSlot.MaybeArmor : null;

                        if (armorItem != null)
                        {
                            int armorBase = ArmorCalculator.GetArmorBase(armorItem);
                            if (armorBase > 0)
                                rdBase = ArmorCalculator.GetBaseRdPercentFromArmorBase(armorBase);
                        }
                        else
                        {
                            var heavyRef = CombatOverhaul.Utils.MarkerRefs.HeavyRef;
                            var mediumRef = CombatOverhaul.Utils.MarkerRefs.MediumRef;
                            if (heavyRef != null && targetDesc.HasFact(heavyRef)) rdBase = 0.40f;
                            else if (mediumRef != null && targetDesc.HasFact(mediumRef)) rdBase = 0.20f;
                        }

                        rd = Mathf.Clamp01(ArmorCalculator.ApplyTypeScaling(rdBase, true)); // físico
                    }
                    else
                    {
                        // === SR 1% por punto ===
                        int sr = SRCalculator.GetTargetSR(targetDesc, ctx);
                        rd = Mathf.Clamp01(sr * 0.01f);
                    }

                    if (rd <= 0f)
                    {
                        factors.Add(1f);
                        continue;
                    }

                    // Aplicar reducción REAL en el pipeline: BonusPercent negativo
                    // Ej.: rd=0.21 → -21%
                    int minusPercent = Mathf.RoundToInt(rd * 100f);
                    comp.BonusPercent -= minusPercent;

                    // Factor para el UI "(x0,xx)"
                    float factor = 1f - Mathf.Clamp01(Mathf.Min(rd, MaxFinalReduction));
                    factors.Add(factor);
                }

                // Entregar factores al parche del log (AppendMultipliers)
                ArmorDR_FactorStore.Set(evt.ParentRule as RuleDealDamage, factors);
            }
            catch
            {
                // silencioso
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) { }
    }
}*/
