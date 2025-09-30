using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using UnityEngine;
using CombatOverhaul.Combat.Calculators;
using CombatOverhaul.Combat.Rules; // ArmorCalculator
using CombatOverhaul.Utils;         // MarkerRefs
using Kingmaker.UnitLogic;          // HasFact()

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
                    // Tu lógica actual para armadura real
                    int armorBase = ArmorCalculator.GetArmorBase(armorItem);
                    if (armorBase <= 0) return;

                    rdBase = ArmorCalculator.GetBaseRdPercentFromArmorBase(armorBase);
                }
                else
                {
                    // Sin armadura: mirar feats Medium/Heavy (instancias canónicas)
                    var desc = target.Descriptor;
                    if (desc == null) return;

                    var heavyRef = MarkerRefs.HeavyRef;
                    var mediumRef = MarkerRefs.MediumRef;

                    if (heavyRef != null && desc.HasFact(heavyRef))
                        rdBase = 0.40f;    // Heavy
                    else if (mediumRef != null && desc.HasFact(mediumRef))
                        rdBase = 0.20f;    // Medium

                    if (rdBase <= 0f) return; // sin marcador: no RD
                }

                var factors = new List<float>(list.Count);

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];
                    int finalNow = dv.FinalValue;
                    if (finalNow <= 0) continue;

                    bool isPhysical = ArmorCalculator.IsPhysical(dv);

                    // mismo escalado que ya usas para armaduras
                    float appliedRD = ArmorCalculator.ApplyTypeScaling(rdBase, isPhysical);

                    float factor = Mathf.Clamp01(1f - Math.Min(appliedRD, MaxFinalReduction));

                    int targetFinal = Mathf.RoundToInt(finalNow * factor);
                    int reduction = dv.Reduction;
                    int newVWR = Math.Max(0, targetFinal + reduction);

                    list[i] = new DamageValue(dv.Source, newVWR, dv.RollAndBonusValue, dv.RollResult, dv.TacticalCombatDRModifier);

                    factors.Add(factor);
                }

                if (factors.Count > 0)
                    ArmorDR_FactorStore.Set(__instance.ParentRule, factors);
            }
            catch (Exception) { /* silencioso */ }
        }
    }
}
