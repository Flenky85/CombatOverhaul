using CombatOverhaul.Calculators;
using CombatOverhaul.Patches.UI.Mana;
using CombatOverhaul.Resources;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using TurnBased.Controllers;
using UnityEngine;

namespace CombatOverhaul.Patches.Combat
{
    [HarmonyPatch(typeof(CombatController), "HandleCombatStart")]
    internal static class InitManaOnCombatStart
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

                var res = ManaResourceBP.Mana;
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
                            SetResourceAmountUnsafe(coll, res, startCur); 

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

        private static void SetResourceAmountUnsafe(UnitAbilityResourceCollection coll, BlueprintScriptableObject res, int value)
        {
            try
            {
                if (coll == null || res == null) return;

                if (!coll.ContainsResource(res))
                    coll.Add(res, restoreAmount: false);

                Dictionary<BlueprintScriptableObject, UnitAbilityResource> map = coll.m_Resources;
                if (!map.TryGetValue(res, out UnitAbilityResource uar) || uar == null)
                    return;

                int old = uar.Amount;
                uar.Amount = Math.Max(0, value);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] SetResourceAmountUnsafe EX: {ex}");
            }
        }
    }
}
