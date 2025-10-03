/*
using HarmonyLib;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic.Mechanics; // MechanicsContext
using UnityEngine;
using CombatOverhaul.Utils;               // Log
using CombatOverhaul.Combat.Calculators;  // ArmorCalculator.IsPhysical
using System;

namespace CombatOverhaul.Patches.Magic
{
    [HarmonyPatch(typeof(RuleCalculateDamage))]
    static class Patch_MagicSR_BeforeDifficulty
    {
        private const float MaxFinalReduction = 1.0f;

        [HarmonyPatch("ApplyDifficultyForDamageReduction")]
        [HarmonyPrefix]
        static void Prefix(RuleCalculateDamage __instance)
        {
            Log.Info("[MagicSR] Prefix enter");

            try
            {
                if (__instance == null) { Log.Info("[MagicSR] __instance == null -> return"); return; }

                var list = __instance.CalculatedDamage;
                if (list == null) { Log.Info("[MagicSR] list == null -> return"); return; }
                if (list.Count == 0) { Log.Info("[MagicSR] list.Count == 0 -> return"); return; }

                var target = __instance.Target;
                var targetDesc = target?.Descriptor;
                if (targetDesc == null) { Log.Info("[MagicSR] targetDesc == null -> return"); return; }

                var reason = __instance.ParentRule?.Reason;
                MechanicsContext ctx = reason?.Context;

                string casterName = __instance.Initiator?.CharacterName ?? __instance.Initiator?.Blueprint?.name ?? "?";
                string targetName = target?.CharacterName ?? target?.Blueprint?.name ?? "?";
                Log.Info($"[MagicSR] caster={casterName} target={targetName} ctx={(ctx == null ? "NULL" : "OK")} damages={list.Count}");

                // === SR efectiva del objetivo ===
                int srValue = 0;
                var upr = targetDesc.Get<UnitPartSpellResistance>();
                if (upr != null)
                {
                    try { srValue = upr.GetValue(ctx); } catch { srValue = 0; }
                    Log.Info($"[MagicSR] UnitPartSpellResistance.GetValue(ctx) = {srValue}");
                    if (srValue <= 0)
                    {
                        try
                        {
                            int fb = upr.GetValue(null);
                            Log.Info($"[MagicSR] UnitPartSpellResistance.GetValue(null) = {fb}");
                            if (fb > srValue) srValue = fb;
                        }
                        catch { }
                    }
                }
                else
                {
                    Log.Info("[MagicSR] UnitPartSpellResistance == null");
                }

                if (srValue <= 0) { Log.Info("[MagicSR] srValue <= 0 -> return (sin reducción)"); return; }

                float rdBase = Mathf.Clamp01(srValue * 0.01f);
                Log.Info($"[MagicSR] srValue={srValue} -> rdBase={rdBase:P0}");

                int affected = 0;

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];
                    string srcType = dv.Source?.GetType()?.Name ?? "null";
                    bool isPhysical = ArmorCalculator.IsPhysical(dv);

                    Log.Info($"[MagicSR] idx={i} src={srcType} isPhysical={(isPhysical ? "YES" : "NO")} FinalValue={dv.FinalValue} ReductionField={dv.Reduction}");

                    if (isPhysical)
                    {
                        Log.Info($"[MagicSR] idx={i} skip physical");
                        continue;
                    }

                    int finalNow = dv.FinalValue;
                    if (finalNow <= 0)
                    {
                        Log.Info($"[MagicSR] idx={i} FinalValue <= 0 -> skip");
                        continue;
                    }

                    float appliedRD = rdBase; // no hay escalado por tipo extra; ya filtramos físicos
                    float factor = Mathf.Clamp01(1f - Mathf.Min(appliedRD, MaxFinalReduction));
                    int targetFinal = Mathf.RoundToInt(finalNow * factor);
                    int newVWR = Math.Max(0, targetFinal + dv.Reduction);

                    Log.Info($"[MagicSR] idx={i} apply rd={appliedRD:P0} factor={factor:F3} => targetFinal={targetFinal} newVWR={newVWR}");

                    list[i] = new DamageValue(
                        dv.Source,
                        newVWR,
                        dv.RollAndBonusValue,
                        dv.RollResult,
                        dv.TacticalCombatDRModifier
                    );

                    affected++;
                }

                Log.Info($"[MagicSR] done. affected={affected}/{list.Count}");
            }
            catch (Exception ex)
            {
                Log.Error("[MagicSR] Exception", ex);
            }
        }
    }
}
*/