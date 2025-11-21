using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class CommunityDomainBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CommunityDomainBaseAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "You can touch a creature as a standard action to heal it of 1d6 points of damage + 1 point per level " +
                    "in the class that gave you access to this domain. This touch also removes the fatigued, " +
                    "shaken, and sickened conditions (but has no effect on more severe conditions)."
                )
                .Configure();
        }
    }
}
