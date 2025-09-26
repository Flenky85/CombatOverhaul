using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using UnityEngine;
using CombatOverhaul.Combat.Calculators;
using CombatOverhaul.Combat.Rules; // ArmorCalculator

namespace CombatOverhaul.Patches.DamageReduction
{
    /// RD por armadura ANTES de la dificultad (global).
    /// - 5% por punto de armadura base.
    /// - 100% contra físico (P/S/B), 50% contra el resto.
    [HarmonyPatch(typeof(RuleCalculateDamage))]
    static partial class Patch_ArmorDR_BeforeDifficulty
    {
        private const float MaxFinalReduction = 1.0f; // 1.0f = sin tope
        private const string LOGTAG = "[CO][ArmorDR] ";

        [HarmonyPatch("ApplyDifficultyForDamageReduction")]
        [HarmonyPrefix]
        static void Prefix(RuleCalculateDamage __instance)
        {
            try
            {
                if (__instance == null)
                {
                    Debug.Log(LOGTAG + "Prefix: __instance NULL");
                    return;
                }

                var list = __instance.CalculatedDamage;
                if (list == null)
                {
                    Debug.Log(LOGTAG + "Prefix: CalculatedDamage NULL");
                    return;
                }

                if (list.Count == 0)
                {
                    Debug.Log(LOGTAG + "Prefix: CalculatedDamage vacío");
                    return;
                }

                var target = __instance.Target;
                var armorItem = target != null && target.Body != null && target.Body.Armor != null
                                ? target.Body.Armor.MaybeArmor
                                : null;

                int armorBase = ArmorCalculator.GetArmorBase(armorItem);
                float rdBase = ArmorCalculator.GetBaseRdPercentFromArmorBase(armorBase);

                Debug.Log($"{LOGTAG}Prefix: target={(target != null ? target.CharacterName : "NULL")} listCount={list.Count} armorBase={armorBase} rdBase={rdBase:0.###}");

                if (armorBase <= 0)
                {
                    Debug.Log(LOGTAG + "Prefix: armorBase <= 0, no aplico RD");
                    return;
                }

                var factors = new List<float>();

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];
                    int finalNow = dv.FinalValue;
                    if (finalNow <= 0) continue;

                    bool isPhysical = ArmorCalculator.IsPhysical(dv);
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
            catch (Exception ex)
            {
                Debug.LogError(LOGTAG + "Prefix EX: " + ex);
            }
        }
    }
}
