using CombatOverhaul.Features;
using CombatOverhaul.Magic.UI;
using CombatOverhaul.Utils; // <-- PartyUtils
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using TurnBased.Controllers;
using UnityEngine;

namespace CombatOverhaul.Magic.EventBus
{
    internal sealed class ManaRegenOnRoundStart :
        ITurnBasedModeHandler, IGlobalSubscriber, ISubscriber
    {
        private const string LogPrefix = "[CO][Mana][Regen/RoundStart]";
        private static BlueprintAbilityResource ManaRes => ManaResource.Mana;

        public void HandleSurpriseRoundStarted() => TryRegenForParty();
        public void HandleRoundStarted(int round) => TryRegenForParty();

        public void HandleTurnStarted(UnitEntityData unit) { /* noop */ }
        public void HandleUnitControlChanged(UnitEntityData unit) { /* noop */ }
        public void HandleUnitNotSurprised(UnitEntityData unit, RuleSkillCheck check) { /* noop */ }

        private static void TryRegenForParty()
        {
            try
            {
                var g = Game.Instance;
                if (g == null) return;

                if (!(g.Player?.IsInCombat ?? false)) return;

                var party = g.Player.PartyAndPets;
                if (party == null) return;

                foreach (var u in EnumerateEligibleUnits(party))
                    TryApplyRegenOnce(u);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{LogPrefix} EX: {ex}");
            }
        }

        private static IEnumerable<UnitEntityData> EnumerateEligibleUnits(IEnumerable<UnitEntityData> units)
        {
            foreach (var u in units)
            {
                if (u == null) continue;
                if (!PartyUtils.IsPartyUnitInCombat(u)) continue;

                var state = u.Descriptor?.State;
                if (state == null || state.IsDead || state.IsFinallyDead) continue;

                yield return u;
            }
        }

        private static void TryApplyRegenOnce(UnitEntityData unit)
        {
            try
            {
                if (ManaRes == null) { Debug.LogWarning($"{LogPrefix} ManaRes null"); return; }

                var coll = unit.Descriptor?.Resources;
                if (coll == null) return;

                EnsureResourceRegistered(coll, ManaRes);

                int max = ManaCalc.CalcMaxMana(unit);
                int regen = ManaCalc.CalcManaPerTurn(unit, max);
                int cur = coll.GetResourceAmount(ManaRes);

                if (max > 0 && regen > 0)
                {
                    int next = Mathf.Clamp(cur + regen, 0, max);
                    SetResourceAmountUnsafe(coll, ManaRes, next);
                    cur = next;
                }

                ManaEvents.Raise(unit, cur, max);
#if DEBUG
                Debug.Log($"{LogPrefix} {unit.CharacterName}: +{regen} => {cur}/{max}");
#endif
            }
            catch (Exception ex)
            {
                Debug.LogError($"{LogPrefix} ApplyRegenOnce EX ({unit?.CharacterName}): {ex}");
            }
        }

        private static void EnsureResourceRegistered(UnitAbilityResourceCollection coll, BlueprintAbilityResource res)
        {
            if (!coll.ContainsResource(res))
                coll.Add(res, restoreAmount: false);
        }

        private static void SetResourceAmountUnsafe(UnitAbilityResourceCollection coll, BlueprintAbilityResource res, int value)
        {
            try
            {
                if (coll == null || res == null) return;
                EnsureResourceRegistered(coll, res);

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
