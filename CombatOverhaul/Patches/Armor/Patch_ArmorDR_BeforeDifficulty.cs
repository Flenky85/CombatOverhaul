using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using UnityEngine;                          // Mathf.RoundToInt
using CombatOverhaul.Combat.Calculators;    // ArmorCalculator
using CombatOverhaul.Combat.Rules;          // ArmorDR_FactorStore
using CombatOverhaul.Utils;                 // MarkerRefs
using Kingmaker.UnitLogic;                  // HasFact()

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

                // === Obtener armadura (si la hay) ===
                var armorSlot = target.Body?.Armor;
                ItemEntityArmor armorItem = (armorSlot != null && armorSlot.HasArmor) ? armorSlot.MaybeArmor : null;

                // === rdBase: de armadura real o de marcador (heavy/medium) ===
                float rdBase = 0f;

                if (armorItem != null)
                {
                    int armorBase = ArmorCalculator.GetArmorBase(armorItem);
                    if (armorBase <= 0) return; // no aporta nada
                    rdBase = ArmorCalculator.GetBaseRdPercentFromArmorBase(armorBase); // p.ej. 5% por punto
                }
                else
                {
                    var desc = target.Descriptor;
                    if (desc == null) return;

                    var heavyRef = MarkerRefs.HeavyRef;
                    var mediumRef = MarkerRefs.MediumRef;

                    if (heavyRef != null && desc.HasFact(heavyRef)) rdBase = 0.40f; // Heavy
                    else if (mediumRef != null && desc.HasFact(mediumRef)) rdBase = 0.20f; // Medium
                    else return; // sin marcador ni armadura
                }

                if (rdBase <= 0f) return;

                // Factor constante para todo daño físico
                float appliedRD = rdBase > MaxFinalReduction ? MaxFinalReduction : rdBase;
                float factorPhysical = 1f - appliedRD;
                if (factorPhysical < 0f) factorPhysical = 0f;
                else if (factorPhysical > 1f) factorPhysical = 1f;

                // Preparar factores sólo si se aplica RD a algo
                List<float> factors = null;
                bool anyApplied = false;

                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var dv = list[i];
                    int finalNow = dv.FinalValue;
                    if (finalNow <= 0)
                    {
                        // Sólo creamos factors si ya se aplicó algo antes
                        if (factors != null) factors.Add(1f);
                        continue;
                    }

                    // Sólo para daño físico; lo mágico se ignora aquí
                    if (!ArmorCalculator.IsPhysical(dv))
                    {
                        if (factors != null) factors.Add(1f);
                        continue;
                    }

                    // A partir de aquí vamos a aplicar: aseguramos lista factors
                    if (factors == null) factors = new List<float>(count);

                    int targetFinal = Mathf.RoundToInt(finalNow * factorPhysical);
                    int reduction = dv.Reduction;
                    int newVWR = Math.Max(0, targetFinal + reduction);

                    if (newVWR != (dv.ValueWithoutReduction)) anyApplied = true;

                    // Sustituimos el DamageValue con el nuevo VWR
                    list[i] = new DamageValue(dv.Source, newVWR, dv.RollAndBonusValue, dv.RollResult, dv.TacticalCombatDRModifier);

                    // Guardamos el factor aplicado para este índice
                    factors.Add(factorPhysical);
                }

                // Si no hemos aplicado RD a ningún entry, no guardamos factors
                if (anyApplied && factors != null && factors.Count > 0)
                {
                    // Si hubo entradas no físicas o sin cambio y no añadimos factor en su momento,
                    // completamos con 1f para mantener longitud
                    while (factors.Count < count) factors.Add(1f);

                    ArmorDR_FactorStore.Set(__instance.ParentRule, factors);
                }
            }
            catch (Exception)
            {
                // silencioso en release
            }
        }
    }
}
