using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class SpikeGrowthAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SpikeGrowth)
                 .EditComponent<AbilityEffectRunAction>(c =>
                 {
                     var spawn = (ContextActionSpawnAreaEffect)c.Actions.Actions[0];
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
                    "Any ground-covering vegetation in the spell's area becomes very hard and sharply pointed without changing " +
                    "its appearance. In areas of bare earth, roots and rootlets act in the same way. Any creature moving on foot " +
                    "into or through the spell's area takes 1d4 points of piercing damage for each 5 feet of movement through the " +
                    "spiked area.\n" +
                    "Any creature that takes damage from this spell must also succeed at a Reflex save or suffer injuries to its " +
                    "feet and legs that slow its base speed by half.This speed penalty lasts for 2d3 rounds."
                )
                .Configure();
        }
    }
}
