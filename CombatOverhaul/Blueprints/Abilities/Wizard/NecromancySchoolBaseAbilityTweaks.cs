using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class NecromancySchoolBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.NecromancySchoolBaseAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can make a melee touch attack that causes a living creature to become shaken " +
                    "for a number of rounds equal to 1/2 your wizard level (minimum 1). If you touch a shaken creature with " +
                    "this ability, it becomes frightened for 1 round if it has fewer Hit Dice than your wizard level."
                )
                .Configure();
        }
    }
}
