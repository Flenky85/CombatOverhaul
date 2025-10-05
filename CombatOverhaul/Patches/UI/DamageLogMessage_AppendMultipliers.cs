using CombatOverhaul.Rules;
using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings.GameLog; 
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.Common;
using System.Text;
using UnityEngine;

namespace CombatOverhaul.Patches.UI
{
    [HarmonyPatch(typeof(DamageLogMessage))]
    static class DamageLogMessage_AppendMultipliers
    {
        [HarmonyPatch("AppendMultipliers")]
        [HarmonyPostfix]
        static void Postfix(StringBuilder sb, RuleDealDamage rule, ref bool __result)
        {
            try
            {
                if (DamageReduction_FactorStore.TryDequeue(rule, out float factor))
                {
                    if (factor > 0f && factor < 0.9999f)
                    {
                        sb.AppendMultiplier(factor);
                        __result = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[ArmorDR-Tooltip] Postfix AppendMultipliers EX: " + ex);
            }
        }
    }
}
