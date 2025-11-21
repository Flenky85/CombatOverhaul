using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Cleric
{
    [AutoRegister]
    internal static class DestructionDomainGreaterToggleAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.DestructionDomainGreaterToggleAbility)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Swift)
                .SetDescriptionValue(
                    "At 8th level, you can emit a 30-foot aura of destruction. All attacks made against targets in this aura " +
                    "(including you) gain a morale bonus on damage equal to 1/2 your level in the class that gave you access to " +
                    "this domain, and all critical threats are automatically confirmed. These rounds do not need to be consecutive.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the aura is active, it consumes 1 charge each round.\n" +
                    "While the aura is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
