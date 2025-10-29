using CombatOverhaul.Guids;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic; // UnitAbilityResourceCollection / UnitAbilityResource
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CombatOverhaul.Resources
{
    /// <summary>
    /// Regenera Ability Resources concretos al inicio de cada ronda para unidades elegibles.
    /// Mantén aquí todas las reglas por resource (GUID) para escalar fácilmente.
    /// </summary>
    internal static class AbilityResourceRegenOnNewRound
    {
        private const string LogPrefix = "[CO][ResRegen]";

        // =============== REGLAS =================
        // Añade aquí más reglas (GUID + cantidad fija / % máximo).
        private static readonly List<RegenRule> _rules = new List<RegenRule>
        {
            // Ejemplo: Lay On Hands +1 por ronda si lo tiene y no está al máximo
            new RegenRule(AbilitiesResourcesGuids.LayOnHands, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.SmiteEvil, flat: 1, percentOfMax: 0f),
        };

        // ============== API =====================
        /// <summary>
        /// Comprueba elegibilidad y aplica regeneraciones configuradas.
        /// Llamar desde NewRoundSweep por cada unidad.
        /// </summary>
        public static void TryApply(UnitEntityData unit)
        {
            if (unit?.Descriptor == null) return;

            foreach (var rule in _rules)
            {
                try { ApplyRule(unit, rule); }
                catch (Exception ex)
                {
                    Debug.LogError($"{LogPrefix} ApplyRule EX ({unit?.CharacterName}, {rule.Guid}): {ex}");
                }
            }
        }

        // ============== IMPLEMENTACIÓN ===========
        private static void ApplyRule(UnitEntityData unit, RegenRule rule)
        {
            var bp = GetResourceBlueprint(rule.Guid);
            if (bp == null) return;

            var col = unit.Descriptor.Resources; // UnitAbilityResourceCollection
            if (col == null) return;

            // 1) ¿La unidad TIENE el resource?
            if (!col.ContainsResource(bp)) return;

            // 2) ¿Está lleno ya?
            if (col.HasMaxAmount(bp)) return;

            // 3) Cantidad actual
            _ = col.GetResourceAmount(bp);

            // 4) Calcular cuánto regenerar
            int regenFlat = Math.Max(0, rule.Flat);
            int regenPct = 0;

            if (rule.PercentOfMax > 0f)
            {
                // Intentamos obtener el MAX vía reflexión:
                if (TryGetMaxAmount(unit, col, bp, out int max) && max > 0)
                {
                    regenPct = Mathf.RoundToInt(max * rule.PercentOfMax);
                }
                // Si falla, seguimos con flat solamente (Restore clamp a máx igualmente)
            }

            int regen = regenFlat + regenPct;
            if (regen <= 0) return;

            // 5) Aplicar (Restore ya clamp a max internamente)
            col.Restore(bp, regen);
        }

        // ============== SOPORTE ==================
        private readonly struct RegenRule
        {
            public readonly string Guid;
            public readonly int Flat;
            public readonly float PercentOfMax;
            public RegenRule(string guid, int flat = 0, float percentOfMax = 0f)
            {
                Guid = guid;
                Flat = flat;
                PercentOfMax = percentOfMax;
            }
        }

        private static readonly Dictionary<string, BlueprintScriptableObject> _bpCache =
            new Dictionary<string, BlueprintScriptableObject>(StringComparer.OrdinalIgnoreCase);

        private static BlueprintScriptableObject GetResourceBlueprint(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid)) return null;
            if (_bpCache.TryGetValue(guid, out var cached)) return cached;
            try
            {
                // Sirve cualquier BlueprintScriptableObject; el de recurso es un subtipo.
                var bp = ResourcesLibrary.TryGetBlueprint<BlueprintScriptableObject>(guid);
                if (bp != null) _bpCache[guid] = bp;
                return bp;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{LogPrefix} TryGetBlueprint EX ({guid}): {ex}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene el máximo del resource: col.GetResource(bp) -> UnitAbilityResource.GetMaxAmount(owner)
        /// </summary>
        private static bool TryGetMaxAmount(UnitEntityData unit, UnitAbilityResourceCollection col, BlueprintScriptableObject bp, out int max)
        {
            max = 0;
            if (unit == null || col == null || bp == null) return false;

            try
            {
                // GetResource es private en el juego original, pero a menudo está publicizado.
                // Aun así lo buscamos por reflexión para soportar ambos casos.
                var tCol = typeof(UnitAbilityResourceCollection);
                var miGetResource = tCol.GetMethod("GetResource",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, new[] { typeof(BlueprintScriptableObject) }, null);
                if (miGetResource == null) return false;

                var resObj = miGetResource.Invoke(col, new object[] { bp });
                if (resObj == null) return false;

                // UnitAbilityResource.GetMaxAmount(UnitDescriptor)
                var tRes = resObj.GetType();
                var miGetMaxAmount = tRes.GetMethod("GetMaxAmount",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, new[] { typeof(UnitDescriptor) }, null);
                if (miGetMaxAmount == null) return false;

                max = (int)miGetMaxAmount.Invoke(resObj, new object[] { unit.Descriptor });
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{LogPrefix} TryGetMaxAmount EX ({unit?.CharacterName}): {ex}");
                return false;
            }
        }
    }
}
