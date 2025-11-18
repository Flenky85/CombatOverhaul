using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class IllusionSchoolGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.IllusionSchoolGreaterAbility)
                .SetDescriptionValue(
                    "At 8th level, you can make yourself invisible as a swift action. " +
                    "This otherwise functions as invisibility, greater." +
                    "Invisivility field starts with 3 charges and gains 1 additional charge every 5 levels; you can spend 1 charge for each round " +
                    "the ability is active, and you regain 1 charge each round while the ability is not active."
                )
                .Configure();
        }
    }
}
