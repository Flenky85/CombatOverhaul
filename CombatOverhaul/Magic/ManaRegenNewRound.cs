using CombatOverhaul.Features;
using CombatOverhaul.Magic.UI.ManaDisplay;   // ManaEvents
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using UnityEngine;

namespace CombatOverhaul.Magic
{
    /// <summary>
    /// Lógica de regeneración de maná, separada del bus.
    /// </summary>
    internal static class ManaRegenOnNewRound
    {
        private const string LogPrefix = "[CO][Mana][Regen/NewRound]";
        private static BlueprintAbilityResource ManaRes => ManaResource.Mana;

        /// <summary>
        /// Elegibilidad: solo tiene sentido si el cálculo da capacidad/ritmo positivos.
        /// Mantiene el comportamiento del original (no “inventa” recursos si no hay max/regen).
        /// </summary>
        public static bool IsEligible(UnitEntityData unit)
        {
            try
            {
                if (unit == null || unit.Descriptor == null || ManaRes == null) return false;

                int max = ManaCalc.CalcMaxMana(unit);
                if (max <= 0) return false;

                int regen = ManaCalc.CalcManaPerTurn(unit, max);
                return regen > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Aplica exactamente la misma lógica que tenías en el archivo original.
        /// </summary>
        public static void Apply(UnitEntityData unit)
        {
            try
            {
                if (unit == null) return;
                if (ManaRes == null)
                {
                    Debug.LogWarning($"{LogPrefix} ManaRes null");
                    return;
                }

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
                Debug.LogError($"{LogPrefix} Apply EX ({unit?.CharacterName}): {ex}");
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
