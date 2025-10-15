using CombatOverhaul.Features;
using CombatOverhaul.Magic.UI;
using CombatOverhaul.Utils; 
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities;
using UnityEngine;

namespace CombatOverhaul.Magic.Patch
{
    [HarmonyPatch]
    internal static class ManaCostRuntime
    {
        private static bool HasEnoughMana(UnitEntityData unit, int cost)
        {
            var res = ManaResource.Mana;
            var coll = unit?.Descriptor?.Resources;
            if (res == null || coll == null) return false;
            if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);
            return coll.GetResourceAmount(res) >= cost;
        }

        private static void SpendMana(UnitEntityData unit, int cost)
        {
            var res = ManaResource.Mana;
            var coll = unit?.Descriptor?.Resources;
            if (res == null || coll == null) return;
            if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);

            int max = ManaCalc.CalcMaxMana(unit);
            int cur = coll.GetResourceAmount(res);
            int after = Mathf.Clamp(cur - cost, 0, max);

            var map = coll.m_Resources;
            if (map != null && map.TryGetValue(res, out var uar) && uar != null)
                uar.Amount = after;

            ManaEvents.Raise(unit, after, max);
        }

        // ==========================================================================   
        // 1) Block casting when not enough mana by reducing AvailableCount to 0.    
        //    Cantrips are excluded because TryFromAbility returns false for level 0.
        // ==========================================================================   
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.GetAvailableForCastCount))]
        private static class AbilityData_GetAvailableForCastCount_Patch
        {
            static void Postfix(AbilityData __instance, ref int __result)
            {
                try
                {
                    if (__result <= 0) return;
                    if (!PartyUtils.IsPartyCasterInCombat(__instance)) return;

                    if (!ManaCosts.TryFromAbility(__instance, out var cost)) return;
                    if (cost <= 0) return;

                    var caster = __instance.Caster?.Unit;
                    if (caster == null) return;

                    if (!HasEnoughMana(caster, cost))
                        __result = 0;
                }
                catch { /* swallow */ }
            }
        }

        // =================================================
        // 2) Spend mana on cast: AbilityData.Spend()      
        //    Cantrips are excluded by TryFromAbility.     
        // =================================================
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.Spend))]
        private static class AbilityData_Spend_Patch
        {
            static void Postfix(AbilityData __instance)
            {
                try
                {
                    if (!PartyUtils.IsPartyCasterInCombat(__instance)) return;
                    if (!ManaCosts.TryFromAbility(__instance, out var cost)) return;
                    if (cost <= 0) return;

                    var caster = __instance.Caster?.Unit;
                    if (caster == null) return;

                    var res = ManaResource.Mana;
                    var coll = caster.Descriptor?.Resources;
                    if (res == null || coll == null) return;
                    if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);

                    int cur = coll.GetResourceAmount(res);
                    if (cur < cost) return;

                    SpendMana(caster, cost);
                }
                catch { /* swallow */ }
            }
        }
    }
}
