using CombatOverhaul.Magic;
using CombatOverhaul.Resources;
using CombatOverhaul.Utils;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatOverhaul.CombatState
{
    internal sealed class NewRoundSweep :
        ITurnBasedModeHandler, IGlobalSubscriber, ISubscriber
    {
        private const string LogPrefix = "[CO][NewRound]";

        public static event Action OnSurpriseRoundStarted;
        public static event Action<int> OnRoundStarted;
        public static event Action<UnitEntityData> OnUnitNewRound;

        private static readonly List<Func<UnitEntityData, bool>> _extraPredicates = new List<Func<UnitEntityData, bool>>();

        public static void RegisterPredicate(Func<UnitEntityData, bool> predicate)
        {
            if (predicate != null) _extraPredicates.Add(predicate);
        }

        public void HandleSurpriseRoundStarted()
        {
            try { OnSurpriseRoundStarted?.Invoke(); }
            catch (Exception ex) { Debug.LogError($"{LogPrefix} Surprise evt EX: {ex}"); }

            SweepUnits();
        }

        public void HandleRoundStarted(int round)
        {
            try { OnRoundStarted?.Invoke(round); }
            catch (Exception ex) { Debug.LogError($"{LogPrefix} Round evt EX: {ex}"); }

            SweepUnits();
        }

        public void HandleTurnStarted(UnitEntityData unit) { /* noop */ }
        public void HandleUnitControlChanged(UnitEntityData unit) { /* noop */ }
        public void HandleUnitNotSurprised(UnitEntityData unit, RuleSkillCheck check) { /* noop */ }

        private static void SweepUnits()
        {
            try
            {
                var g = Game.Instance;
                if (g == null) return;
                if (!(g.Player?.IsInCombat ?? false)) return;

                var party = g.Player.PartyAndPets;
                if (party == null) return;

                foreach (var u in EnumerateEligibleUnits(party))
                {
                    try
                    {
                        if (ManaRegenOnNewRound.IsEligible(u))
                            ManaRegenOnNewRound.Apply(u);
                        AbilityResourceRegenOnNewRound.TryApply(u);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"{LogPrefix} ProcessUnit EX ({u?.CharacterName}): {ex}");
                    }

                    try { OnUnitNewRound?.Invoke(u); }
                    catch (Exception ex) { Debug.LogError($"{LogPrefix} Handler EX ({u?.CharacterName}): {ex}"); }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"{LogPrefix} Sweep EX: {ex}");
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

                if (!PassesExtraPredicates(u)) continue;

                yield return u;
            }
        }

        private static bool PassesExtraPredicates(UnitEntityData u)
        {
            if (_extraPredicates.Count == 0) return true;
            foreach (var p in _extraPredicates)
            {
                try { if (!p(u)) return false; }
                catch (Exception ex)
                {
                    Debug.LogError($"{LogPrefix} Predicate EX ({u?.CharacterName}): {ex}");
                }
            }
            return true;
        }
    }
}
