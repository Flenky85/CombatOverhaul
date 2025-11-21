using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Cleric
{
    [AutoRegister]
    internal static class ReposeDomainGreaterToggleAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.ReposeDomainGreaterToggleAbility)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Swift)
                .SetDescriptionValue(
                    "At 8th level, you can emit a 30-foot aura that wards against death. Living creatures in this " +
                    "area are immune to all death effects, energy drain, and effects that cause negative levels. " +
                    "This ward does not remove negative levels that a creature has already gained, but the negative " +
                    "levels have no effect while the creature is inside the warded area.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the aura is active, it consumes 1 charge each round.\n" +
                    "While the aura is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
