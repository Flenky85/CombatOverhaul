using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class UniversalistSchoolQuickenAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.UniversalistSchoolQuickenAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 8; 
                })
                .SetDescriptionValue(
                    "At 20th level, you can quicken your next spell as though using the Quicken Spell feat. " +
                    "Costs 8 charges of metamagic mastery."
                )
                .Configure();
        }
    }
}
