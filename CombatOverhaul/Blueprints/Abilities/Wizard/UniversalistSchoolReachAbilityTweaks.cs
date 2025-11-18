using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class UniversalistSchoolReachAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.UniversalistSchoolReachAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 2; 
                })
                .SetDescriptionValue(
                    "The next spell the wizard casts will be altered as though using the Reach Spell feat. " +
                    "Costs 2 charges of metamagic mastery."
                )
                .Configure();
        }
    }
}
