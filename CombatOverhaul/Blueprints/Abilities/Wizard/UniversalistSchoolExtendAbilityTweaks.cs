using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class UniversalistSchoolExtendAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.UniversalistSchoolExtendAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 2; 
                })
                .SetDescriptionValue(
                    "The next spell the wizard casts will be cast as if extended as though using the Extend Spell feat. " +
                    "Costs 2 charges of metamagic mastery."
                )
                .Configure();
        }
    }
}
