using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class PillarOfLifeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PillarOfLife)
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
                    "You conjure a pillar of positive energy in a single 5-foot area within range that " +
                    "radiates light as if it were a sunrod. Living creatures adjacent to the pillar can heal " +
                    "1d3 points of damage per caster level (maximum 12d3). Creatures can move into the space containing the pillar, " +
                    "but if an undead creature moves into the pillar it takes 1d6 points of damage per caster " +
                    "level (maximum 12d6). A creature cannot benefit or suffer more than once from a " +
                    "single casting of this spell."
                )
                .Configure();
        }
    }
}
