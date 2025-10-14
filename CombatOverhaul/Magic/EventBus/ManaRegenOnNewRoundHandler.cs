using CombatOverhaul.Features;
using CombatOverhaul.Magic.UI;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Linq;
using UnityEngine;

namespace CombatOverhaul.Magic.EventBus
{
    internal sealed class ManaRegenOnRoundStart :
        ITurnBasedModeHandler, IGlobalSubscriber, ISubscriber
    {
        public void HandleSurpriseRoundStarted() => DoRegenForParty();

        public void HandleRoundStarted(int round) => DoRegenForParty();

        public void HandleTurnStarted(UnitEntityData unit) { /* noop */ }
        public void HandleUnitControlChanged(UnitEntityData unit) { /* noop */ }
        public void HandleUnitNotSurprised(UnitEntityData unit, RuleSkillCheck check) { /* noop */ }

        private static void DoRegenForParty()
        {
            try
            {
                var g = Game.Instance;
                if (g == null || !g.Player.IsTurnBasedModeOn() || !g.Player.IsInCombat) return;

                var party = g.Player?.PartyAndPets?
                    .Where(u => u != null && u.IsInCombat && u.IsPlayerFaction
                        && !u.Descriptor.State.IsDead && !u.Descriptor.State.IsFinallyDead)
                    .ToList();
                if (party == null || party.Count == 0) return;

                foreach (var unit in party) ApplyRegenOnce(unit);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana][Regen/RoundStart] EX: {ex}");
            }
        }

        private static void ApplyRegenOnce(UnitEntityData unit)
        {
            var res = ManaResource.Mana;
            if (res == null) { Debug.Log("[CO][Mana] Resource null"); return; }

            var coll = unit.Descriptor?.Resources;
            if (coll == null) return;
            if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);

            int max = ManaCalc.CalcMaxMana(unit);
            int regen = ManaCalc.CalcManaPerTurn(unit, max);
            int curBefore = coll.GetResourceAmount(res);
            int curAfter = curBefore;

            if (max > 0 && regen > 0)
            {
                curAfter = Mathf.Clamp(curBefore + regen, 0, max);
                SetResourceAmount(coll, ManaResource.Mana, curAfter);
            }

            ManaEvents.Raise(unit, curAfter, max);
            Debug.Log($"[CO][Mana][Regen/RoundStart] {unit.CharacterName}: {curBefore} + {regen} => {curAfter} / max={max}");
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
