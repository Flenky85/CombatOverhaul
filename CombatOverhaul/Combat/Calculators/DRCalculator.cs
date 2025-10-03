/*using CombatOverhaul.Utils;          // MarkerRefs (ya lo tienes)
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using UnityEngine;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class DRCalculator
    {
        private const float MaxFinalReduction = 1.0f;

        /// Devuelve el % de reducción a aplicar a ESTE DamageValue:
        /// - Físico: usa armadura/markers (tu lógica)
        /// - No físico: usa SR (1% por punto)
        public static float GetReductionPercentForDamage(UnitEntityData target, UnitDescriptor targetDesc,
                                                         MechanicsContext ctx, DamageValue dv)
        {
            bool isPhysical = ArmorCalculator.IsPhysical(dv);
            if (isPhysical)
            {
                // === ARMOR ===
                // 1) Lee armadura equipada
                var armorSlot = target?.Body?.Armor;
                ItemEntityArmor armorItem = armorSlot != null && armorSlot.HasArmor ? armorSlot.MaybeArmor : null;

                float rdBase = 0f;
                if (armorItem != null)
                {
                    int armorBase = ArmorCalculator.GetArmorBase(armorItem);
                    if (armorBase <= 0) return 0f;
                    rdBase = ArmorCalculator.GetBaseRdPercentFromArmorBase(armorBase);
                }
                else
                {
                    // 2) Markers Medium / Heavy
                    if (targetDesc == null) return 0f;
                    var heavyRef = MarkerRefs.HeavyRef;
                    var mediumRef = MarkerRefs.MediumRef;

                    if (heavyRef != null && targetDesc.HasFact(heavyRef)) rdBase = 0.40f;
                    else if (mediumRef != null && targetDesc.HasFact(mediumRef)) rdBase = 0.20f;
                    else return 0f;
                }

                // Escalado por tipo (tu función)
                float appliedRD = ArmorCalculator.ApplyTypeScaling(rdBase, isPhysical: true);
                return Mathf.Clamp01(Mathf.Min(appliedRD, MaxFinalReduction));
            }
            else
            {
                // === SR ===
                int sr = SRCalculator.GetTargetSR(targetDesc, ctx);
                if (sr <= 0) return 0f;
                float rdBase = Mathf.Clamp01(sr * 0.01f);
                return Mathf.Clamp01(Mathf.Min(rdBase, MaxFinalReduction));
            }
        }
    }
}
*/