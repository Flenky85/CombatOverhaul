using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Cleric
{
    [AutoRegister]
    internal static class ArtificeDomainBaseToggleAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.ArtificeDomainBaseToggleAbility)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Swift)
                .SetDescriptionValue(
                    "You can emit a 30-foot-radius aura that grants your allies a +4 bonus on all saving throws against " +
                    "effects that inflict the fatigued or exhausted condition.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the aura is active, it consumes 1 charge each round.\n" +
                    "While the aura is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
