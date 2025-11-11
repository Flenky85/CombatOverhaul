using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.ActivatableAbilities
{
    [AutoRegister]
    internal static class ShamanBoneSpiritTrueAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.ShamanBoneSpiritTrueAbility)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Swift)
                .SetDescriptionValue(
                    "As a swift action, the shaman sheds her body and becomes incorporeal. While in this form, all of " +
                    "her weapon attacks are considered to have the ghost touch weapon special ability.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the form is active, it consumes 1 charge each round.\n" +
                    "While the form is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
