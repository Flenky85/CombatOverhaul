using CombatOverhaul.Features;
using CombatOverhaul.Magic;
using CombatOverhaul.Magic.UI.ManaDisplay;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using TurnBased.Controllers;
using UnityEngine;

namespace CombatOverhaul.CombatState.Patch
{
    [HarmonyPatch(typeof(CombatController), "HandleCombatStart")]
    internal static class CombatStart
    {
        static void Postfix()
        {
            try
            {
                var game = Game.Instance;
                if (game == null || game.Player == null)
                {                
                    return;
                }

                var party = game.Player.PartyAndPets;
                if (party == null)
                {
                    return;
                }

                var res = ManaResource.Mana;
                if (res == null)
                {
                    return;
                }

                int processed = 0;

                for (int i = 0; i < party.Count; i++)
                {
                    var unit = party[i];
                    if (!IsEligiblePlayerInCombat(unit)) continue;

                    try
                    {
                        int maxDyn, startCur;
                        maxDyn = ManaCalc.CalcMaxMana(unit);
                        startCur = maxDyn; 

                        var coll = unit.Descriptor.Resources;
                        if (!coll.ContainsResource(res))
                            coll.Add(res, restoreAmount: false);

                        int curBefore = coll.GetResourceAmount(res);
                        if (curBefore > 0) coll.Spend(res, curBefore); 

                        if (startCur > 0)
                            SetResourceAmount(coll, ManaResource.Mana, startCur); 

                        int curAfter = coll.GetResourceAmount(res);
                        
                        ManaEvents.Raise(unit, curAfter, maxDyn); 

                        Debug.Log($"[CO][Mana] Init '{unit.CharacterName}': curBefore={curBefore} -> curAfter={curAfter}, maxDyn={maxDyn} (FULL)");
                        processed++;
                    }
                    catch (Exception exUnit)
                    {
                        Debug.LogError($"[CO][Mana] Error initializing unit '{unit?.CharacterName ?? "NULL"}': {exUnit}");
                    }
                }

                Debug.Log($"[CO][Mana] HandleCombatStart: Done. Units processed={processed}.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] HandleCombatStart: EX {ex}");
            }
        }

        private static bool IsEligiblePlayerInCombat(UnitEntityData unit)
        {
            if (unit == null) return false;
            if (!unit.IsInGame) return false;
            if (!unit.IsInCombat) return false;
            if (!unit.IsPlayerFaction) return false;
            return true;
        }

        private static void SetResourceAmount(UnitAbilityResourceCollection coll, BlueprintAbilityResource res, int value)
        {
            if (coll == null || res == null) return;

            if (!coll.ContainsResource(res))
                coll.Add(res, restoreAmount: false);

            var map = coll.m_Resources;
            if (map == null) return;

            if (!map.TryGetValue(res, out var uar) || uar == null) return;

            uar.Amount = Math.Max(0, value);
        }

    }
}
