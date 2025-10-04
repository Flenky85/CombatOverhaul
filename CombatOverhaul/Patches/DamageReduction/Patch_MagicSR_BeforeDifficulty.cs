using HarmonyLib;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic.Mechanics;
using UnityEngine; // Debug, Mathf
using System;
using CombatOverhaul.Combat.Calculators; // MagicSrCalc

namespace CombatOverhaul.Patches.DamageReduction
{
    [HarmonyPatch(typeof(RuleCalculateDamage))]
    static class Patch_MagicSR_BeforeDifficulty
    {
        private const string TAG = "[CO][SRDMG] ";

        [HarmonyPatch("ApplyDifficultyForDamageReduction")]
        [HarmonyPrefix]
        static void Prefix(RuleCalculateDamage __instance)
        {
            try
            {
                Debug.Log(TAG + "Prefix ENTER");

                var list = __instance?.CalculatedDamage;
                var target = __instance?.Target;
                var init = __instance?.Initiator;
                var ctx = __instance?.Reason?.Context;

                Debug.Log(TAG + "list=" + (list != null ? list.Count.ToString() : "null")
                               + "  target=" + (target != null)
                               + "  initiator=" + (init != null)
                               + "  ctx=" + (ctx != null));

                if (list == null || list.Count == 0 || target == null)
                {
                    Debug.Log(TAG + "Abort: list empty/null or target null");
                    return;
                }

                var p = target.Get<UnitPartSpellResistance>();
                if (p == null)
                {
                    Debug.Log(TAG + "Abort: UnitPartSpellResistance null");
                    return;
                }

                // SR/DEF: con contexto devuelve DÉFICIT; sin contexto devuelve SR cruda
                int srOrDef = MagicSrCalc.ComputeSrDeficitNoRoll(ctx, target, init);

                // Inmunidad detectable solo con contexto (el helper devuelve int.MaxValue)
                bool immune = (ctx != null) && srOrDef == int.MaxValue;

                Debug.Log(TAG + (ctx != null
                                ? ("WITH CTX -> deficit=" + (immune ? "IMMUNE" : srOrDef.ToString()))
                                : ("NO CTX -> srRaw=" + srOrDef)));

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];
                    int baseFinal = dv.FinalValue;
                    if (baseFinal <= 0)
                    {
                        Debug.Log(TAG + "[" + i + "] skip (FinalNow=" + baseFinal + ")");
                        continue;
                    }

                    bool isPhysical = dv.Source is PhysicalDamage;
                    Debug.Log(TAG + "[" + i + "] type=" + (isPhysical ? "PHYS" : "NON-PHYS") + "  FinalNow=" + baseFinal);

                    if (isPhysical) continue; // solo no-físico

                    float percent = Mathf.Clamp(srOrDef, 0, 100) / 100f;
                    float factor = immune ? 0f : (1f - percent);

                    int targetFinal = Mathf.RoundToInt(baseFinal * factor);
                    int newVWR = Math.Max(0, targetFinal + dv.Reduction);

                    Debug.Log(TAG + "[" + i + "] immune=" + immune + "  percent=" + (percent * 100f).ToString("0")
                                   + "%  factor=" + factor.ToString("0.000") + "  newVWR=" + newVWR + "  red=" + dv.Reduction);

                    list[i] = new DamageValue(dv.Source, newVWR, dv.RollAndBonusValue, dv.RollResult, dv.TacticalCombatDRModifier);
                }

                Debug.Log(TAG + "Prefix EXIT");
            }
            catch (Exception ex)
            {
                Debug.Log(TAG + "EX: " + ex);
            }
        }
    }
}
