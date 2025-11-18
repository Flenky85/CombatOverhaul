using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class UniversalistSchoolMaximizeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.UniversalistSchoolMaximizeAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6; 
                })
                .SetDescriptionValue(
                    "At 16th level, you can maximize your next spell as though using the Maximize Spell feat. " +
                    "Costs 6 charges of metamagic mastery."
                )
                .Configure();
        }
    }
}
