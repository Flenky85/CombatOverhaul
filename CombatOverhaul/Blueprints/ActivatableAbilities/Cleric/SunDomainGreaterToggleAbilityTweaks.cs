using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Cleric
{
    [AutoRegister]
    internal static class SunDomainGreaterToggleAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.SunDomainGreaterToggleAbility)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Swift)
                .SetDescriptionValue(
                    "At 8th level, you can emit a 30-foot nimbus of light. Any hostile creature within this radius must succeed " +
                    "at a Fortitude save to resist the effects of this aura. If the creature fails, it is blinded until it leaves " +
                    "the area of the spell. A creature that has resisted the effect cannot be affected again by this particular aura. " +
                    "In addition, undead within this radius take an amount of damage equal to your level in the class that gave you " +
                    "access to this domain each round that they remain inside the nimbus. These rounds do not need to be consecutive.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the aura is active, it consumes 1 charge each round.\n" +
                    "While the aura is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
