using System;
using System.Text;
using HarmonyLib;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using UnityEngine;

namespace CombatOverhaul.Patches
{
    /// RD por armadura ANTES de la dificultad (global).
    /// - 5% por punto de armadura base.
    /// - 100% contra físico (P/S/B), 50% contra el resto.
    [HarmonyPatch(typeof(RuleCalculateDamage))]
    static class Patch_ArmorDR_BeforeDifficulty
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

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];

                    int finalNow = dv.FinalValue;
                    if (finalNow <= 0)
                    {
                        Debug.Log($"{LOGTAG} i={i} FinalValue<=0 (saltado)");
                        continue;
                    }

                    bool isPhysical = dv.Source is PhysicalDamage;
                    float appliedRD = isPhysical ? rdBase : rdBase * 0.5f;
                    float factor = 1f - Mathf.Min(appliedRD, MaxFinalReduction);
                    factor = Mathf.Clamp01(factor);

                    int targetFinal = Mathf.RoundToInt(finalNow * factor);
                    int reduction = dv.Reduction;
                    int newVWR = Mathf.Max(0, targetFinal + reduction);

                    // Log por entrada
                    Debug.Log(
                      $"{LOGTAG} i={i} type={(isPhysical ? "PHYS" : "NONPHYS")} " +
                      $"finalNow={finalNow} reduction={reduction} factor={factor:0.###} " +
                      $"targetFinal={targetFinal} newVWR={newVWR}"
                    );

                    // Reconstrucción del DamageValue
                    var newDV = new DamageValue(
                      dv.Source,
                      newVWR,
                      dv.RollAndBonusValue,
                      dv.RollResult,
                      dv.TacticalCombatDRModifier
                    );

                    list[i] = newDV;
                }
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
                var bp = armorItem?.Blueprint;                         // BlueprintItemArmor
                if (bp == null) return 0;

                // 1) Bono base del tipo de armadura (lo que queremos)
                //   Ej.: Full Plate ⇒ ~9; Chain Shirt ⇒ ~4, etc.
                int fromType = 0;
                try { fromType = bp.Type != null ? bp.Type.ArmorBonus : 0; } catch { }

                // 2) Bono en el propio item (normalmente 0 en vanilla para "base")
                int fromItem = 0;
                try { fromItem = bp.ArmorBonus; } catch { }

                // Logging útil para verificar
                try
                {
                    var typeName = bp.Type != null ? bp.Type.name : "NULL";
                    Debug.Log($"[CO][ArmorDR] Armor blueprint: {bp.name} | Type={typeName} | Type.ArmorBonus={fromType} | Item.ArmorBonus={fromItem}");
                }
                catch {  }

                // Preferimos el valor del TIPO; si no hay, caemos al del item
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
