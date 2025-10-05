using CombatOverhaul.Rules;
using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings.GameLog; // DamageLogMessage
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.Common;
using System.Text;
using UnityEngine;

namespace CombatOverhaul.Patches.Armor
{
    // Pega tras tu clase ArmorDR_FactorStore
    [HarmonyPatch(typeof(DamageLogMessage))]
    static class Patch_DamageLogMessage_AppendMultipliers
    {
        // Firma original: bool AppendMultipliers(StringBuilder sb, RuleDealDamage rule, DamageValue damage)
        [HarmonyPatch("AppendMultipliers")]
        [HarmonyPostfix]
        static void Postfix(StringBuilder sb, RuleDealDamage rule, DamageValue damage, ref bool __result)
        {
            try
            {
                float factor;
                if (ArmorDR_FactorStore.TryDequeue(rule, out factor))
                {
                    // Evita ruido si por alguna razón llega 1.0
                    if (factor > 0f && factor < 0.9999f)
                    {
                        // Usa el helper del juego para dar el formato "x0,55"
                        sb.AppendMultiplier(factor);
                        // Marcamos que hubo multiplicadores para que el llamador añada paréntesis si procede
                        __result = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[CO][ArmorDR-Tooltip] Postfix AppendMultipliers EX: " + ex);
            }
        }
    }
}
