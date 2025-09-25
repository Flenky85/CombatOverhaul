using HarmonyLib;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatOverhaul.Patches
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
                var armorItem = target?.Body?.Armor?.MaybeArmor;
                int armorBase = GetArmorBase(armorItem);
                float rdBase = Mathf.Max(0f, armorBase * 0.05f);

                Debug.Log($"{LOGTAG}Prefix: target={(target?.CharacterName ?? "NULL")} listCount={list.Count} armorBase={armorBase} rdBase={rdBase:0.###}");

                if (armorBase <= 0)
                {
                    Debug.Log(LOGTAG + "Prefix: armorBase <= 0, no aplico RD");
                    return;
                }

                // Acumulamos líneas para mostrarlas luego en el tooltip del combat log
                var lines = new List<string>();
                var factors = new List<float>();

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];
                    int finalNow = dv.FinalValue;
                    if (finalNow <= 0) continue;

                    bool isPhysical = dv.Source is PhysicalDamage;
                    float appliedRD = isPhysical ? rdBase : rdBase * 0.5f;
                    float factor = Mathf.Clamp01(1f - Mathf.Min(appliedRD, MaxFinalReduction));

                    // recalcula ValueWithoutReduction como ya haces…
                    int targetFinal = Mathf.RoundToInt(finalNow * factor);
                    int reduction = dv.Reduction;
                    int newVWR = Mathf.Max(0, targetFinal + reduction);

                    list[i] = new DamageValue(dv.Source, newVWR, dv.RollAndBonusValue, dv.RollResult, dv.TacticalCombatDRModifier);

                    // guarda el factor para el tooltip
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

        private static int GetArmorBase(ItemEntityArmor armorItem)
        {
            try
            {
                var bp = armorItem?.Blueprint; // BlueprintItemArmor
                if (bp == null) return 0;

                int fromType = 0;
                try { fromType = bp.Type != null ? bp.Type.ArmorBonus : 0; } catch { }

                int fromItem = 0;
                try { fromItem = bp.ArmorBonus; } catch { }

                try
                {
                    var typeName = bp.Type != null ? bp.Type.name : "NULL";
                    Debug.Log($"[CO][ArmorDR] Armor blueprint: {bp.name} | Type={typeName} | Type.ArmorBonus={fromType} | Item.ArmorBonus={fromItem}");
                }
                catch { }

                return fromType > 0 ? fromType : fromItem;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[CO][ArmorDR] GetArmorBase EX: " + ex);
                return 0;
            }
        }
    }
}
