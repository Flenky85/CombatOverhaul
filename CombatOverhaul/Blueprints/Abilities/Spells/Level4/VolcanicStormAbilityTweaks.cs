using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class VolcanicStormAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.VolcanicStorm)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 10;
                })

                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var coldPerLevel = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Fire
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.Default
                            },
                            BonusValue = new ContextValue
                            {
                                ValueType = ContextValueType.Simple,
                                Value = 0
                            }
                        },
                        Half = false,
                        HalfIfSaved = false,
                        IsAoE = true
                    };

                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[] { coldPerLevel }
                    };
                })

                .EditComponent<AdditionalAbilityEffectRunActionOnClickedTarget>(comp =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)comp.Action.Actions[0];

                    spawn.DurationValue.Rate = DurationRate.Rounds;
                    spawn.DurationValue.DiceType = DiceType.D3;
                    spawn.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    spawn.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "Chunks of hot volcanic rock and clumps of ash pound down when this spell is cast, dealing " +
                    "1d4 points of fire damage per caster level (maximum 10d4) to every creature in the area. This " +
                    "damage only occurs once, when the spell is cast.For the remaining duration of the spell, heavy " +
                    "ash rains down in the area.Creatures inside this area take a –4 penalty on Perception skill " +
                    "checks and the entire area is treated as difficult terrain.At the end of the duration, the rock " +
                    "and ash disappear, leaving no aftereffects(other than the damage dealt)."
                )
                .Configure();
        }
    }
}
