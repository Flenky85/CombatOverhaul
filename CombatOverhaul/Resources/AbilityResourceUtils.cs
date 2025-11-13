using CombatOverhaul.Guids;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CombatOverhaul.Resources
{
    internal static class AbilityResourceRegenOnNewRound
    {
        private const string LogPrefix = "[CO][ResRegen]";

        private static readonly List<RegenRule> _rules = new List<RegenRule>
        {
            //Paladin
            new RegenRule(AbilitiesResourcesGuids.LayOnHands, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.SmiteEvil, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.DivineGuardianTroth, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ChannelEnergyHospitaler, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.Stonestrike, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.AllIsDarkness, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.TorturedCrusadeLayOnHands, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShiningLight, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.WeaponBond, flat: 1, percentOfMax: 0f, 
                BuffsGuids.WeaponBondBuff),
            new RegenRule(AbilitiesResourcesGuids.StonelordDefesniveStance, flat: 1, percentOfMax: 0f, 
                BuffsGuids.StonelordDefensiveStanceBuff, BuffsGuids.Fatigued, BuffsGuids.Exhausted),
            new RegenRule(AbilitiesResourcesGuids.MartyrPerformance, flat: 1, percentOfMax: 0f, 
                BuffsGuids.MartyrGreatnessBuff, BuffsGuids.MartyrCourageBuff, BuffsGuids.MartyrHeroicsBuff),

            //Shaman
            new RegenRule(AbilitiesResourcesGuids.ShamanBoneSpiritBaseResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanBoneSpiritGreaterResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanFLameSpiritTrueResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanFLameSpiritGreaterResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanFlameSpiritBaseResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanStonesSpiritTrueResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanStonesSpiritGreaterResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanStonesSpiritBaseResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanWavesSpiritTrueResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanWavesSpiritGreaterResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanWavesSpiritBaseResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanWindSpiritTrueResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanWindSpiritGreaterResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanWindSpiritBaseResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanFrostSpiritTrueResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanFrostSpiritGreaterResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanFrostSpiritBaseResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanLifeSpiritChannelEnergyResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanLifeSpiritTrueResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanNatureSpiritBaseResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShadowShamanShadowsResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.SPiritWardenChannelResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.WitchDoctorChannelEnergyResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanPlagueSpiritBaseResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ShamanHexAuraOfPurityResource, flat: 1, percentOfMax: 0f,
                BuffsGuids.ShamanHexAuraOfPurityBuff),
            new RegenRule(AbilitiesResourcesGuids.ShamanHexAirBarrierResource, flat: 1, percentOfMax: 0f,
                BuffsGuids.ShamanHexAirBarrierBuff),
            new RegenRule(AbilitiesResourcesGuids.ShamanBattleSpiritTrueResource, flat: 1, percentOfMax: 0f,
                BuffsGuids.ParagonBattleBuff),
            new RegenRule(AbilitiesResourcesGuids.ShamanBattleSpiritResource, flat: 1, percentOfMax: 0f,
                BuffsGuids.ShamanBattleSpiritBuff),
            new RegenRule(AbilitiesResourcesGuids.ShamanBattleSpiritGreaterResource, flat: 1, percentOfMax: 0f,
                BuffsGuids.ShamanBattleSpiritGreaterBuffControl),
            new RegenRule(AbilitiesResourcesGuids.ShamanBoneSpiritTrueResource, flat: 1, percentOfMax: 0f,
                BuffsGuids.ShamanBoneSpiritTrueBuff),
            new RegenRule(AbilitiesResourcesGuids.ShamanWeaponPoolResourse, flat: 1, percentOfMax: 0f,
                BuffsGuids.ShamanWeaponEnchantDurationBuff),

            //Hellknight
            new RegenRule(AbilitiesResourcesGuids.SmiteChaosResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.HellknightDisciplineCommandResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.HellknightDisciplineOnslaughtResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.HellKnightOrderOfTheGodclawResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.HellknightDisciplineTrackResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.HellknightDisciplineWrackResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.HellKnightOrderOfTheRackResource, flat: 1, percentOfMax: 0f),

            //Monk
            new RegenRule(AbilitiesResourcesGuids.KiPowerResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.StunningFistResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.PerfectStrikeResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.ElementalFistAbilityResource, flat: 1, percentOfMax: 0f),
            new RegenRule(AbilitiesResourcesGuids.DrunkenKiPowerResource, flat: 1, percentOfMax: 0f),
        };

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

        private static void ApplyRule(UnitEntityData unit, RegenRule rule)
        {
            var bp = GetResourceBlueprint(rule.Guid);
            if (bp == null) return;

            var col = unit.Descriptor.Resources; 
            if (col == null) return;

            if (!col.ContainsResource(bp)) return;

            if (rule.SkipIfHasBuffGuids != null && rule.SkipIfHasBuffGuids.Any(g => HasBuff(unit, g)))
                return; 

            if (col.HasMaxAmount(bp)) return;

            _ = col.GetResourceAmount(bp);

            int regenFlat = Math.Max(0, rule.Flat);
            int regenPct = 0;

            if (rule.PercentOfMax > 0f)
            {
                if (TryGetMaxAmount(unit, col, bp, out int max) && max > 0)
                {
                    regenPct = Mathf.RoundToInt(max * rule.PercentOfMax);
                }
            }

            int regen = regenFlat + regenPct;
            if (regen <= 0) return;

            col.Restore(bp, regen);
        }

        private readonly struct RegenRule
        {
            public readonly string Guid;
            public readonly int Flat;
            public readonly float PercentOfMax;

            public readonly string[] SkipIfHasBuffGuids;

            public RegenRule(string guid, int flat = 0, float percentOfMax = 0f, params string[] skipIfHasBuffGuids)
            {
                Guid = guid;
                Flat = flat;
                PercentOfMax = percentOfMax;
                SkipIfHasBuffGuids = skipIfHasBuffGuids;
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

        private static bool TryGetMaxAmount(UnitEntityData unit, UnitAbilityResourceCollection col, BlueprintScriptableObject bp, out int max)
        {
            max = 0;
            if (unit == null || col == null || bp == null) return false;

            try
            {
                var tCol = typeof(UnitAbilityResourceCollection);
                var miGetResource = tCol.GetMethod("GetResource",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, new[] { typeof(BlueprintScriptableObject) }, null);
                if (miGetResource == null) return false;

                var resObj = miGetResource.Invoke(col, new object[] { bp });
                if (resObj == null) return false;

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
        private static bool HasBuff(UnitEntityData unit, string buffGuid)
        {
            if (string.IsNullOrWhiteSpace(buffGuid) || unit == null || unit.Descriptor == null || unit.Descriptor.Buffs == null)
                return false;

            try
            {
                var buff = GetBlueprint<BlueprintBuff>(buffGuid);
                return buff != null && unit.Descriptor.Buffs.HasFact(buff);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{LogPrefix} HasBuff EX ({unit?.CharacterName}, {buffGuid}): {ex}");
                return false;
            }
        }
        private static T GetBlueprint<T>(string guid) where T : BlueprintScriptableObject
        {
            if (string.IsNullOrWhiteSpace(guid)) return null;

            if (_bpCache.TryGetValue(guid, out BlueprintScriptableObject cached))
                return cached as T;

            try
            {
                var bp = ResourcesLibrary.TryGetBlueprint<T>(guid);
                if (bp != null) _bpCache[guid] = bp;
                return bp;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{LogPrefix} TryGetBlueprint<{typeof(T).Name}> EX ({guid}): {ex}");
                return null;
            }
        }

    }
}
