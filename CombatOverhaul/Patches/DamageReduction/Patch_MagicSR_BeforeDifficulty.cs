using HarmonyLib;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic.Mechanics;
using UnityEngine; // Debug, Mathf
using System;
using System.Collections.Generic;                // <-- NUEVO
using CombatOverhaul.Combat.Calculators;         // MagicSrCalc
using CombatOverhaul.Combat.Rules;               // <-- NUEVO: ArmorDR_FactorStore

namespace CombatOverhaul.Patches.DamageReduction
{
    [HarmonyPatch(typeof(RuleCalculateDamage))]
    static class Patch_MagicSR_BeforeDifficulty
    {
        private const string TAG = "[CO][SRDMG] ";

        [HarmonyPatch("ApplyDifficultyForDamageReduction")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)] // <-- Importante: corre DESPUÉS del parche de armadura
        static void Prefix(RuleCalculateDamage __instance)
        {
            try
            {
#if DEBUG_CO
                Debug.Log(TAG + "Prefix ENTER");
#endif

                var list = __instance?.CalculatedDamage;
                var target = __instance?.Target;
                var init = __instance?.Initiator;
                var ctx = __instance?.Reason?.Context;

#if DEBUG_CO
                Debug.Log(TAG + "list=" + (list != null ? list.Count.ToString() : "null")
                               + "  target=" + (target != null)
                               + "  initiator=" + (init != null)
                               + "  ctx=" + (ctx != null));
#endif

                if (list == null || list.Count == 0 || target == null)
                    return;

                var p = target.Get<UnitPartSpellResistance>();
                if (p == null)
                    return;

                // SR/DEF: con contexto devuelve DÉFICIT; sin contexto devuelve SR cruda
                int srOrDef = MagicSrCalc.ComputeSrDeficitNoRoll(ctx, target, init);
                bool immune = (ctx != null) && srOrDef == int.MaxValue;

#if DEBUG_CO
                Debug.Log(TAG + (ctx != null
                                ? ("WITH CTX -> deficit=" + (immune ? "IMMUNE" : srOrDef.ToString()))
                                : ("NO CTX -> srRaw=" + srOrDef)));
#endif

                // Lista de factores para el UI: 1f por defecto, factor SR en los mágicos
                var factorsSR = new List<float>(list.Count);
                for (int k = 0; k < list.Count; k++) factorsSR.Add(1f);

                for (int i = 0; i < list.Count; i++)
                {
                    var dv = list[i];
                    int baseFinal = dv.FinalValue;
                    if (baseFinal <= 0)
                        continue;

                    bool isPhysical = dv.Source is PhysicalDamage;
#if DEBUG_CO
                    Debug.Log(TAG + "[" + i + "] type=" + (isPhysical ? "PHYS" : "NON-PHYS") + "  FinalNow=" + baseFinal);
#endif
                    if (isPhysical)
                        continue; // sólo SR sobre NO físico

                    float percent = immune ? 1.0f : Mathf.Clamp(srOrDef, 0, 100) / 100f; // 0.20 = 20%
                    float factor = immune ? 0.0f : (1f - percent);                      // 0.80 = queda 80%

                    // Aplica SR materializando en ValueWithoutReduction (tu enfoque original)
                    int targetFinal = Mathf.RoundToInt(baseFinal * factor);
                    int newVWR = Math.Max(0, targetFinal + dv.Reduction);

                    list[i] = new DamageValue(
                        dv.Source,
                        newVWR,
                        dv.RollAndBonusValue,
                        dv.RollResult,
                        dv.TacticalCombatDRModifier
                    );

                    // Marca factor para el UI en el índice mágico
                    factorsSR[i] = factor; // esto hará que el log pinte (0.80)
                }

                // Publica la lista para este Rule. Al ir con Priority.Last, el UI usará ésta.
                ArmorDR_FactorStore.Set(__instance.ParentRule, factorsSR);

#if DEBUG_CO
                Debug.Log(TAG + "Prefix EXIT");
#endif
            }
            catch (Exception ex)
            {
#if DEBUG_CO
                Debug.Log(TAG + "EX: " + ex);
#endif
            }
        }
    }
}
