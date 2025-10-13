using CombatOverhaul.Calculators;
using CombatOverhaul.Patches.UI.Mana;
using CombatOverhaul.Resources;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.RuleSystem.Rules; // <-- necesario para RuleSkillCheck
using System;
using System.Linq;
using UnityEngine;

namespace CombatOverhaul.Bus
{
    internal sealed class ManaRegenOnRoundStart :
        ITurnBasedModeHandler, IGlobalSubscriber, ISubscriber
    {
        // Ronda sorpresa: trátala igual que una ronda normal
        public void HandleSurpriseRoundStarted() => DoRegenForParty();

        // Ronda normal (1, 2, 3, ...)
        public void HandleRoundStarted(int round) => DoRegenForParty();

        // ====== Métodos requeridos por la interfaz (no los usamos) ======
        public void HandleTurnStarted(UnitEntityData unit) { /* noop */ }
        public void HandleUnitControlChanged(UnitEntityData unit) { /* noop */ }
        public void HandleUnitNotSurprised(UnitEntityData unit, RuleSkillCheck check) { /* noop */ }
        // ================================================================

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
            var res = ManaResourceBP.Mana;
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
                SetResourceAmountUnsafe(coll, res, curAfter);
            }

            ManaEvents.Raise(unit, curAfter, max);
            Debug.Log($"[CO][Mana][Regen/RoundStart] {unit.CharacterName}: {curBefore} + {regen} => {curAfter} / max={max}");
        }

        private static void SetResourceAmountUnsafe(UnitAbilityResourceCollection coll, Kingmaker.Blueprints.BlueprintScriptableObject res, int value)
        {
            try
            {
                if (coll == null || res == null) return;
                if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);

                var map = coll.m_Resources; // AssemblyPublicizer
                if (!map.TryGetValue(res, out UnitAbilityResource uar) || uar == null) return;
                uar.Amount = Math.Max(0, value);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] SetResourceAmountUnsafe EX: {ex}");
            }
        }
    }
}
