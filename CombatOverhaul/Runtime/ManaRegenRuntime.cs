using System;
using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using UnityEngine;
using CombatOverhaul.Calculators;      // CalcMaxMana / CalcManaPerTurn
using CombatOverhaul.Resources;        // ManaResourceBP
using CombatOverhaul.Patches.UI.Mana;  // ManaEvents

namespace CombatOverhaul.Runtime
{
    internal static class ManaRegenRuntime
    {
        /// <summary>
        /// Regenera maná para Party+Pets (jugadores) al empezar una NUEVA RONDA.
        /// - Recalcula Max dinámico con CalcMaxMana(unit)
        /// - Calcula regen = CalcManaPerTurn(unit, max)
        /// - Sube el “actual” sin depender del max nativo
        /// - Lanza ManaEvents.Raise para actualizar la barra
        /// </summary>
        public static void DoPerRoundRegenForParty(string reasonTag = "NewRound")
        {
            try
            {
                var game = Game.Instance;
                if (game == null || game.Player == null)
                {
                    Debug.Log("[CO][Mana] Regen: Game/Player null.");
                    return;
                }

                var party = game.Player.PartyAndPets;
                if (party == null)
                {
                    Debug.Log("[CO][Mana] Regen: PartyAndPets null.");
                    return;
                }

                var res = ManaResourceBP.Mana;
                if (res == null)
                {
                    Debug.Log("[CO][Mana] Regen: Mana resource is null. Did you register it?");
                    return;
                }

                int processed = 0;
                for (int i = 0; i < party.Count; i++)
                {
                    var unit = party[i];
                    if (!IsEligiblePlayerInCombat(unit)) continue;

                    try
                    {
                        // 1) Máx dinámico (puede cambiar por buffs/estados)
                        int maxDyn = ManaCalc.CalcMaxMana(unit);

                        // 2) Actual y regen
                        var coll = unit.Descriptor.Resources;
                        if (!coll.ContainsResource(res))
                            coll.Add(res, restoreAmount: false);

                        int cur = coll.GetResourceAmount(res);
                        int regen = ManaCalc.CalcManaPerTurn(unit, maxDyn);

                        int target = cur + regen;
                        if (maxDyn > 0 && target > maxDyn) target = maxDyn;
                        if (target < 0) target = 0;

                        SetResourceAmountUnsafe(coll, res, target);

                        // 3) Refrescar UI
                        ManaEvents.Raise(unit, target, maxDyn);

                        Debug.Log($"[CO][Mana] Regen[{reasonTag}] '{unit.CharacterName}': cur={cur} + regen={regen} => {target} / maxDyn={maxDyn}");
                        processed++;
                    }
                    catch (Exception exUnit)
                    {
                        Debug.LogError($"[CO][Mana] Regen unit '{unit?.CharacterName ?? "NULL"}' EX: {exUnit}");
                    }
                }

                Debug.Log($"[CO][Mana] Regen[{reasonTag}] done. Units processed={processed}.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] Regen EX: {ex}");
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

        /// Fija el ACTUAL sin clamp nativo (usa m_Resources, requiere AssemblyPublicizer)
        private static void SetResourceAmountUnsafe(UnitAbilityResourceCollection coll, BlueprintScriptableObject res, int value)
        {
            try
            {
                if (coll == null || res == null) return;

                if (!coll.ContainsResource(res))
                    coll.Add(res, restoreAmount: false);

                Dictionary<BlueprintScriptableObject, UnitAbilityResource> map = coll.m_Resources;
                if (!map.TryGetValue(res, out UnitAbilityResource uar) || uar == null) return;

                int old = uar.Amount;
                uar.Amount = Math.Max(0, value);

                // Si quieres notificar bus nativo, descomenta:
                // EventBus.RaiseEvent<IUnitAbilityResourceHandler>(h => h.HandleAbilityResourceChange(coll.m_Owner, uar, old), true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] SetResourceAmountUnsafe EX: {ex}");
            }
        }
    }
}
