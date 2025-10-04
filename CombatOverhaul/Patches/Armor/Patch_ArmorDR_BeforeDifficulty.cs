using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using UnityEngine;
using CombatOverhaul.Combat.Calculators; // ArmorCalculator
using CombatOverhaul.Combat.Rules;
using CombatOverhaul.Utils;              // MarkerRefs
using Kingmaker.UnitLogic;               // HasFact()

namespace CombatOverhaul.Patches.DamageReduction
{
    [HarmonyPatch(typeof(RuleCalculateDamage))]
    static partial class Patch_ArmorDR_BeforeDifficulty
    {
        private const float MaxFinalReduction = 1.0f;

        [HarmonyPatch("ApplyDifficultyForDamageReduction")]
        [HarmonyPrefix]
        static void Prefix(RuleCalculateDamage __instance)
        {
            try
            {
                if (__instance == null) return;

                var list = __instance.CalculatedDamage;
                if (list == null || list.Count == 0) return;

                var target = __instance.Target;
                if (target == null) return;

                // === Obtener armadura de forma segura ===
                var armorSlot = target.Body?.Armor;
                ItemEntityArmor armorItem = (armorSlot != null && armorSlot.HasArmor) ? armorSlot.MaybeArmor : null;

                // === rdBase: de armadura real O de feat marcador ===
                float rdBase = 0f;

                if (armorItem != null)
                {
                    int armorBase = ArmorCalculator.GetArmorBase(armorItem);
                    if (armorBase <= 0) return;
                    rdBase = ArmorCalculator.GetBaseRdPercentFromArmorBase(armorBase); // p. ej. 5% por punto
                }
                else
                {
                    var desc = target.Descriptor;
                    if (desc == null) return;

                    var heavyRef = MarkerRefs.HeavyRef;
                    var mediumRef = MarkerRefs.MediumRef;

                    if (heavyRef != null && desc.HasFact(heavyRef)) rdBase = 0.40f; // Heavy
                    else if (mediumRef != null && desc.HasFact(mediumRef)) rdBase = 0.20f; // Medium

                    if (rdBase <= 0f) return; // sin marcador: no RD
                }

                var factors = new List<float>(list.Count);

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];
                    int finalNow = dv.FinalValue;
                    if (finalNow <= 0) continue;

                    // SOLO aplicamos RD de armadura a daño FÍSICO. No tocamos lo mágico aquí.
                    if (!ArmorCalculator.IsPhysical(dv))
                    {
                        factors.Add(1f); // sin cambio; útil por si otro parche lee estos factores
                        continue;
                    }

                    // factor físico simple: 1 - rdBase (cap)
                    float appliedRD = Mathf.Min(rdBase, MaxFinalReduction);
                    float factor = Mathf.Clamp01(1f - appliedRD);

                    int targetFinal = Mathf.RoundToInt(finalNow * factor);
                    int reduction = dv.Reduction;
                    int newVWR = Math.Max(0, targetFinal + reduction);

                    list[i] = new DamageValue(dv.Source, newVWR, dv.RollAndBonusValue, dv.RollResult, dv.TacticalCombatDRModifier);
                    factors.Add(factor);
                }

                if (factors.Count > 0)
                    ArmorDR_FactorStore.Set(__instance.ParentRule, factors);
            }
            catch (Exception)
            {
                // silencioso
            }
        }
    }
}
