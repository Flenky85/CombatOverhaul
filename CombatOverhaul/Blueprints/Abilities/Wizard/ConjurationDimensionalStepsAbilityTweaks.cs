using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class ConjurationDimensionalStepsAbility
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ConjurationDimensionalStepsAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3; 
                })
                .SetDescriptionValue(
                    "At 8th level, you can use this ability to teleport up to 30 feet as a move action.\n" +
                    "The ability have a cooldown of 3 rounds."
                )
                .Configure();
        }
    }
}
