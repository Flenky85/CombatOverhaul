using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class UniversalistSchoolEmpowerAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.UniversalistSchoolEmpowerAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 4; 
                })
                .SetDescriptionValue(
                    "At 12th level, you can empower your next spell as though using the Empower Spell feat. " +
                    "Costs 4 charges of metamagic mastery."
                )
                .Configure();
        }
    }
}
