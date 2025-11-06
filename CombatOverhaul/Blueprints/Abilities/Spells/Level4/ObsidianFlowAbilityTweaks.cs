using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class ObsidianFlowAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ObsidianFlow)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D4;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs, 
                    m_UseMax = true,
                    m_Max = 10,                                  
                    m_AffectedByIntensifiedMetamagic = false
                })
                .EditComponent<AdditionalAbilityEffectRunActionOnClickedTarget>(c =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)c.Action.Actions[0];
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
                    "You convert a thin layer of the ground to molten glass that cools quickly. Creatures in the area take 1d4 " +
                    "points of fire damage per caster levels (maximum of 10d4) and become entangled until they make a successful " +
                    "Strength, Athletics or Mobility check (the DC equals the spell's saving throw DC). Any creature within the " +
                    "area that makes a successful Reflex save takes half damage and is not entangled.\n" +
                    "The ground is covered with slippery expanses and sharp shards of obsidian.The area of effect is difficult " +
                    "terrain and creatures inside this area take a –5 penalty on Mobility skill checks. A successful DC 15 " +
                    "Mobility check is required to charge across the area. A creature that falls prone in the area takes 1d6 " +
                    "points of damage from sharp obsidian."
                )
                .Configure();
        }
    }
}
