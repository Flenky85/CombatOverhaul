using CombatOverhaul.Armor;
using CombatOverhaul.Guids;
using CombatOverhaul.Magic;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatOverhaul.Damage.Patch
{
    [HarmonyPatch(typeof(RuleCalculateDamage))]
    internal static class AttackDamageReduction
    {
        private const float MaxFinalReduction = 1.0f;

        private static BlueprintUnitFact _sunderArmorFact;
        private static BlueprintUnitFact SunderArmorFact =>
            _sunderArmorFact ??= ResourcesLibrary.TryGetBlueprint<BlueprintUnitFact>(BuffsGuids.SunderArmor);

        [HarmonyPatch("ApplyDifficultyForDamageReduction")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)]
        private static void Prefix(RuleCalculateDamage __instance)
        {
            try
            {
                var list = __instance?.CalculatedDamage;
                var target = __instance?.Target;
                if (list == null || list.Count == 0 || target == null)
                    return;

                var desc = target.Descriptor;
                bool hasSunder = desc != null && SunderArmorFact != null && Has(desc, SunderArmorFact);

                int count = list.Count;

                bool hasPhys = false, hasNonPhys = false;
                for (int i = 0; i < count && (!hasPhys || !hasNonPhys); i++)
                {
                    var dv = list[i];
                    if (dv.FinalValue <= 0) continue;
                    if (ArmorCalculator.IsPhysical(dv)) hasPhys = true; else hasNonPhys = true;
                }
                if (!hasPhys && !hasNonPhys) return;

                float factorPhysical = 1f;
                if (hasPhys)
                {
                    float rdBase = 0f;

                    if (target.Body?.Armor?.MaybeItem is ItemEntityArmor armorItem)
                    {
                        int armorBase = ArmorCalculator.GetArmorBase(armorItem);
                        if (armorBase > 0)
                            rdBase = ArmorCalculator.GetBaseRdPercentFromArmorBase(armorBase);
                    }
                    else
                    {
                        var heavyRef = MarkerRefs.HeavyRef;
                        var mediumRef = MarkerRefs.MediumRef;

                        if (Has(desc, heavyRef)) rdBase = 0.40f; 
                        else if (Has(desc, mediumRef)) rdBase = 0.20f;
                    }

                    if (hasSunder && rdBase > 0f)
                        rdBase *= 0.5f;

                    if (rdBase > 0f)
                    {
                        float clamped = rdBase > MaxFinalReduction ? MaxFinalReduction : rdBase;
                        factorPhysical = Mathf.Clamp01(1f - clamped);
                    }
                }

                
                float factorMagic = 1f;
                if (hasNonPhys)
                {
                    var srPart = target.Get<UnitPartSpellResistance>();
                    if (srPart != null)
                    {
                        UnitEntityData initiator = __instance.Initiator;
                        MechanicsContext ctx = __instance.Reason?.Context;

                        int srOrDef = MagicSrCalc.ComputeSrDeficitNoRoll(ctx, target, initiator);
                        bool immune = ctx != null && srOrDef == int.MaxValue;

                        float percent = immune ? 1.0f : Mathf.Clamp(srOrDef, 0, 100) / 100f; 
                        factorMagic = immune ? 0.0f : 1f - percent;
                    }
                }

                if (Mathf.Approximately(factorPhysical, 1f) && Mathf.Approximately(factorMagic, 1f))
                    return;

                
                List<float> factors = null; 
                bool anyApplied = false;

                for (int i = 0; i < count; i++)
                {
                    var dv = list[i];
                    int finalNow = dv.FinalValue;
                    if (finalNow <= 0)
                    {
                        factors?.Add(1f);
                        continue;
                    }

                    bool isPhys = ArmorCalculator.IsPhysical(dv);
                    float factor = isPhys ? factorPhysical : factorMagic;

                    if (Mathf.Approximately(factor, 1f))
                    {
                        factors?.Add(1f);
                        continue;
                    }

                    if (factors == null)
                    {
                        factors = new List<float>(count);
                        for (int k = 0; k < i; k++) factors.Add(1f);
                    }

                    int targetFinal = Mathf.RoundToInt(finalNow * factor);
                    int newVWR = Math.Max(0, targetFinal + dv.Reduction);

                    list[i] = new DamageValue(
                        dv.Source,
                        newVWR,
                        dv.RollAndBonusValue,
                        dv.RollResult,
                        dv.TacticalCombatDRModifier
                    );

                    factors.Add(factor);
                    anyApplied = true;
                }

                if (anyApplied && factors != null)
                {
                    while (factors.Count < count) factors.Add(1f);
                    DamageReduction_FactorStore.Set(__instance.ParentRule, factors);
                }
            }
            catch
            {
                
            }
        }

        private static bool Has(UnitDescriptor d, BlueprintUnitFact fact)
        {
            if (d == null || fact == null) return false;
            var facts = d.Facts;
            var list = facts?.List;
            if (list == null) return false;

            foreach (var f in list)
            {
                if (f != null && ReferenceEquals(f.Blueprint, fact))
                    return true;
            }
            return false;
        }

        private static bool Has(UnitDescriptor d, BlueprintUnitFactReference reference)
            => reference != null && Has(d, reference.Get());
    }
}
