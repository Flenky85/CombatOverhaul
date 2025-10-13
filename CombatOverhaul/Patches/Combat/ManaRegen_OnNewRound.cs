/*using CombatOverhaul.Calculators;
using CombatOverhaul.Patches.UI.Mana;
using CombatOverhaul.Resources;
using HarmonyLib;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using TurnBased.Controllers;
using UnityEngine;

namespace CombatOverhaul.Patches.Combat
{
    [HarmonyPatch(typeof(TurnController), "Prepare")]
    internal static class RegenManaOnTurnStart_TB
    {
        // Quién ya recibió regen en la "vuelta" actual (solo party)
        private static readonly HashSet<UnitEntityData> GrantedThisSweep = new HashSet<UnitEntityData>();

        static void Postfix(TurnController __instance)
        {
            try
            {
                var unit = __instance?.Rider;
                if (unit == null || !unit.IsInCombat || !unit.IsPlayerFaction) return;

                // Conjunto actual de unidades jugadoras válidas en combate (vivas, presentes)
                var partyNow = SafePartyInCombat();
                if (partyNow.Count == 0) return;

                // Si al cambiar el estado del combate cambia el conjunto (muertes, summons, etc.),
                // sacamos del HashSet los que ya no están.
                PruneGrantedSet(partyNow);

                // ¿Ya recibió regen en esta "vuelta"?
                if (GrantedThisSweep.Contains(unit))
                {
                    Debug.Log($"[CO][Mana][Regen] SKIP (Delay/2º turno en misma ronda): {unit.CharacterName}");
                    // Refrescamos UI con el max dinámico por coherencia (opcional)
                    var res = ManaResourceBP.Mana;
                    if (res != null)
                    {
                        int max = ManaCalc.CalcMaxMana(unit);
                        int cur = unit.Descriptor.Resources.GetResourceAmount(res);
                        ManaEvents.Raise(unit, cur, max);
                    }
                    return;
                }

                // Aplicar regen (1ª vez que entra en Prepare durante esta vuelta)
                ApplyRegenOnce(unit);

                // Añadir al conjunto de "ya concedidos"
                GrantedThisSweep.Add(unit);

                // ¿Hemos concedido a toda la party? -> reseteamos para la próxima ronda lógica
                if (GrantedThisSweep.Count >= partyNow.Count)
                {
                    Debug.Log($"[CO][Mana][Regen] Sweep complete ({GrantedThisSweep.Count}/{partyNow.Count}). Reset for next round.");
                    GrantedThisSweep.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana][Regen] EX: {ex}");
            }
        }

        private static void ApplyRegenOnce(UnitEntityData unit)
        {
            var res = ManaResourceBP.Mana;
            if (res == null)
            {
                Debug.Log("[CO][Mana][Regen] Resource is null. Register ManaResourceBP first.");
                return;
            }

            var coll = unit.Descriptor?.Resources;
            if (coll == null) return;

            if (!coll.ContainsResource(res))
                coll.Add(res, restoreAmount: false);

            int max = ManaCalc.CalcMaxMana(unit);
            int regen = ManaCalc.CalcManaPerTurn(unit, max);

            int curBefore = coll.GetResourceAmount(res);
            int curAfter = curBefore;

            if (max > 0 && regen > 0)
            {
                curAfter = Mathf.Clamp(curBefore + regen, 0, max);
                SetResourceAmountUnsafe(coll, res, curAfter);
            }

            ManaEvents.Raise(unit, curAfter, max);
            Debug.Log($"[CO][Mana][Regen] {unit.CharacterName}: {curBefore} + {regen} => {curAfter} / max={max}");
        }

        // Bypass limpio del max blueprint (tenemos el max a 0 y hacemos clamp nosotros)
        private static void SetResourceAmountUnsafe(UnitAbilityResourceCollection coll, Kingmaker.Blueprints.BlueprintScriptableObject res, int value)
        {
            try
            {
                if (coll == null || res == null) return;

                if (!coll.ContainsResource(res))
                    coll.Add(res, restoreAmount: false);

                var map = coll.m_Resources; // AssemblyPublicizer
                if (!map.TryGetValue(res, out UnitAbilityResource uar) || uar == null) return;

                uar.Amount = Math.Max(0, value);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] SetResourceAmountUnsafe EX: {ex}");
            }
        }

        private static List<UnitEntityData> SafePartyInCombat()
        {
            try
            {
                var g = Game.Instance;
                var list = g?.Player?.PartyAndPets;
                if (list == null) return new List<UnitEntityData>();
                // Sólo los que están realmente en combate y en facción del jugador
                return list.Where(u => u != null && u.IsInCombat && u.IsPlayerFaction).ToList();
            }
            catch { return new List<UnitEntityData>(); }
        }

        private static void PruneGrantedSet(List<UnitEntityData> partyNow)
        {
            if (GrantedThisSweep.Count == 0) return;
            for (var it = GrantedThisSweep.GetEnumerator(); it.MoveNext();)
            {
                var u = it.Current;
                if (u == null || !partyNow.Contains(u))
                {
                    // no podemos borrar durante la enumeración con foreach; hacemos después
                }
            }
            // Eficiente: reconstruimos solo con los presentes
            GrantedThisSweep.RemoveWhere(u => u == null || !partyNow.Contains(u));
        }
    }
}*/
