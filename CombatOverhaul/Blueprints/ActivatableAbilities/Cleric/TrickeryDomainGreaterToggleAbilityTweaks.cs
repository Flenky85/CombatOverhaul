using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Cleric
{
    [AutoRegister]
    internal static class TrickeryDomainGreaterToggleAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.TrickeryDomainGreaterToggleAbility)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Swift)
                .SetDescriptionValue(
                    "At 8th level, you can make yourself and any number of allies within 30 feet invisible.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the aura is active, it consumes 1 charge each round.\n" +
                    "While the aura is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
