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

                // === Early-out previo: si NO hay daño físico con FinalValue>0, salimos ya ===
                bool hasPhysical = false;
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var dv0 = list[i];
                    if (dv0.FinalValue > 0 && ArmorCalculator.IsPhysical(dv0)) { hasPhysical = true; break; }
                }
                if (!hasPhysical) return;

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
                float factorPhysical = Mathf.Clamp01(1f - (rdBase > MaxFinalReduction ? MaxFinalReduction : rdBase));

                // Preparar factores sólo si se aplica RD a algo;
                // usamos un contador para pre-rellenar 1f y evitar un while de relleno posterior.
                List<float> factors = null;
                bool anyApplied = false;
                int pendingOnes = 0;

                for (int i = 0; i < count; i++)
                {
                    var dv = list[i];
                    int finalNow = dv.FinalValue;

                    if (finalNow <= 0)
                    {
                        if (factors != null) factors.Add(1f); else pendingOnes++;
                        continue;
                    }

                    // Sólo para daño físico; lo mágico se ignora aquí
                    if (!ArmorCalculator.IsPhysical(dv))
                    {
                        if (factors != null) factors.Add(1f); else pendingOnes++;
                        continue;
                    }

                    // A partir de aquí vamos a aplicar: aseguramos lista factors
                    if (factors == null)
                    {
                        factors = new List<float>(count);
                        // pre-rellenamos los "1f" que quedaron pendientes antes de crear la lista
                        if (pendingOnes > 0) { for (int k = 0; k < pendingOnes; k++) factors.Add(1f); }
                    }

                    int targetFinal = Mathf.RoundToInt(finalNow * factorPhysical);
                    int reduction = dv.Reduction;
                    int newVWR = Math.Max(0, targetFinal + reduction);

                    if (newVWR != dv.ValueWithoutReduction) anyApplied = true;

                    // Sustituimos el DamageValue con el nuevo VWR
                    list[i] = new DamageValue(dv.Source, newVWR, dv.RollAndBonusValue, dv.RollResult, dv.TacticalCombatDRModifier);

                    // Guardamos el factor aplicado para este índice
                    factors.Add(factorPhysical);
                }

                // Si no hemos aplicado RD a ningún entry, no guardamos factors
                if (anyApplied && factors != null && factors.Count > 0)
                {
                    // Si todavía faltan posiciones (poco probable), prellenamos
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
