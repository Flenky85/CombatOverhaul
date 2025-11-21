using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class RuneDomainGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.RuneDomainGreaterAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6; 
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "At 8th level, you can create a warding rune in a desired location. Any creature entering a 5-foot area around " +
                    "the rune must succeed on a Will save or be unable to attack for 2d3 rounds. The rune lasts for a number of rounds " +
                    "equal to your level in that class.\n" +
                    "The ability have a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
