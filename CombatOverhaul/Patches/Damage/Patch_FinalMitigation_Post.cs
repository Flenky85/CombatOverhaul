/*
using CombatOverhaul.Combat.Calculators; // SRCalculator, ArmorCalculator
using CombatOverhaul.Combat.Rules;       // ArmorDR_FactorStore
using HarmonyLib;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatOverhaul.Patches.Damage
{
    [HarmonyPatch(typeof(RuleCalculateDamage))]
    static class Patch_FinalMitigation_Post
    {
        private const float MaxFinalReduction = 1.0f;

        // ⬇️ ESTE ES EL CAMBIO CLAVE: OnTrigger en vez de ApplyDifficultyForDamageReduction
        [HarmonyPatch("OnTrigger")]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        static void Postfix(RuleCalculateDamage __instance)
        {
            try
            {
                var list = __instance?.CalculatedDamage;
                var target = __instance?.Target;
                var targetDesc = target?.Descriptor;
                if (list == null || list.Count == 0 || targetDesc == null) return;

                var reason = __instance.ParentRule?.Reason;
                MechanicsContext ctx = reason?.Context;

                var factors = new List<float>(list.Count);

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];
                    int beforeFinal = dv.FinalValue;
                    if (beforeFinal <= 0) { factors.Add(1f); continue; }

                    bool isPhysical = ArmorCalculator.IsPhysical(dv);
                    float red = 0f;

                    if (isPhysical)
                    {
                        // === tu lógica de armadura inline ===
                        var armorSlot = target?.Body?.Armor;
                        var armorItem = (armorSlot != null && armorSlot.HasArmor) ? armorSlot.MaybeArmor : null;
                        float rdBase = 0f;

                        if (armorItem != null)
                        {
                            int armorBase = ArmorCalculator.GetArmorBase(armorItem);
                            if (armorBase > 0) rdBase = ArmorCalculator.GetBaseRdPercentFromArmorBase(armorBase);
                        }
                        else
                        {
                            var heavyRef = CombatOverhaul.Utils.MarkerRefs.HeavyRef;
                            var mediumRef = CombatOverhaul.Utils.MarkerRefs.MediumRef;
                            if (heavyRef != null && targetDesc.HasFact(heavyRef)) rdBase = 0.40f;
                            else if (mediumRef != null && targetDesc.HasFact(mediumRef)) rdBase = 0.20f;
                        }

                        red = Mathf.Clamp01(ArmorCalculator.ApplyTypeScaling(rdBase, true)); // físico
                    }
                    else
                    {
                        // === SR 1% por punto ===
                        int sr = CombatOverhaul.Combat.Calculators.SRCalculator.GetTargetSR(targetDesc, ctx);
                        red = Mathf.Clamp01(sr * 0.01f);
                    }

                    if (red <= 0f) { factors.Add(1f); continue; }

                    float factor = 1f - Mathf.Clamp01(Mathf.Min(red, MaxFinalReduction));
                    int targetFinal = Mathf.RoundToInt(beforeFinal * factor);

                    // sumar nuestra reducción a Reduction para que el UI muestre el paréntesis
                    int extraReduced = Math.Max(0, beforeFinal - targetFinal);
                    int mergedReduction = dv.Reduction + extraReduced;
                    int newVWR = Math.Max(0, targetFinal + mergedReduction);

                    list[i] = new DamageValue(
                        dv.Source,
                        newVWR,
                        dv.RollAndBonusValue,
                        dv.RollResult,
                        dv.TacticalCombatDRModifier
                    );

                    factors.Add(factor);
                }

                ArmorDR_FactorStore.Set(__instance.ParentRule as RuleDealDamage, factors);
            }
            catch { }
        }
    }
}
*/