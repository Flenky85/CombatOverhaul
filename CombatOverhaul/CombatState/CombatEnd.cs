using CombatOverhaul.Features;
using CombatOverhaul.Magic;
using CombatOverhaul.Magic.UI.ManaDisplay;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;
using TurnBased.Controllers;
using UnityEngine;

namespace CombatOverhaul.CombatState
{
    [HarmonyPatch(typeof(CombatController), nameof(CombatController.HandleCombatEnd))]
    internal static class CombatEnd
    {
        static void Postfix()
        {
            try
            {
                var g = Game.Instance;
                if (g == null || g.Player == null) return;

                var party = g.Player.PartyAndPets;
                var res = ManaResource.Mana;
                if (party == null || res == null) return;

                foreach (var u in party)
                {
                    if (u == null || !u.IsPlayerFaction) continue;

                    var coll = u.Descriptor?.Resources;
                    if (coll == null || !coll.ContainsResource(res)) continue;

                    int maxMana = ManaCalc.CalcMaxMana(u);
                    SetResourceAmount(coll, res, maxMana);     // res ya es ManaResource.Mana
                    ManaEvents.Raise(u, maxMana, maxMana);

                    Debug.Log($"[CO][Mana] CombatEnd -> '{u.CharacterName}' mana = {maxMana}/{maxMana}");

                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] CombatEndPatch EX: {ex}");
            }
        }

        private static void SetResourceAmount(UnitAbilityResourceCollection coll, BlueprintAbilityResource res, int value)
        {
            try
            {
                if (coll == null || res == null) return;
                if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);

                var map = coll.m_Resources;
                if (map == null) return;
                if (!map.TryGetValue(res, out var uar) || uar == null) return;

                uar.Amount = Math.Max(0, value);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] SetResourceAmount EX: {ex}");
            }
        }
    }
}
