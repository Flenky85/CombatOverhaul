using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class DrunkenMasterDrink8thAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DrunkenMasterDrink8thAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var restore = (ContextRestoreResource)c.Actions.Actions[0]; 
                    restore.Value.Value = 4;
                })
                .SetDescriptionValue(
                    "A drunken master consumes 1 bottle of alcohol and gains 4 points of drunken ki."
                )
                .Configure();
        }
    }
}
