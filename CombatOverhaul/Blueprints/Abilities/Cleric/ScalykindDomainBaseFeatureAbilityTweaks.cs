using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class ScalykindDomainBaseFeatureAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ScalykindDomainBaseFeatureAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can activate a gaze attack against enemies within a 30-foot radius. " +
                    "These enemies must make a Will save (DC = 10 + 1/2 your cleric level + your Wisdom modifier). " +
                    "Those who fail take 1d6 points of damage + 1 point for every two cleric levels you possess and " +
                    "are fascinated for one round."
                )
                .Configure();
        }
    }
}
