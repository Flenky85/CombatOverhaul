using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class DarknessDomainGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DarknessDomainGreaterAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[1];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D3; 
                    apply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2 
                    };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "At 8th level, as a standard action you can shoot a blast of divine moonlight from your eyes, " +
                    "as a ranged touch attack against a single target within 30 feet. Moonfire deals 1d8 points of " +
                    "damage per 2 levels in the class that gave you access to this domain, and the target is dazzled " +
                    "for 1 round per level in the class that gave you access to this domain. Moonfire deals 1d10 points " +
                    "of damage per level in the class that gave you access to this domain against lycanthropes." +
                    "The ability have a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
