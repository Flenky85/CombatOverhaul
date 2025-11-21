using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class WeatherDomainBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WeatherDomainBaseAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can create a storm burst targeting any foe within 30 feet as a " +
                    "ranged touch attack. The storm burst deals 1d6 points of damage + 1 point for every two " +
                    "levels you possess in the class that gave you access to this domain. In addition, the target " +
                    "is buffeted by winds and rain, causing it to take a –2 penalty on attack rolls for 1 round."
                )
                .Configure();
        }
    }
}
