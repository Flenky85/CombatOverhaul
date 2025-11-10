using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexEntanglingCurseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexEntanglingCurseAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman entangles a creature within 30 feet for a number of rounds equal to the shaman's " +
                    "Charisma modifier (minimum 1). A successful Reflex saving throw negates this effect. Whether or " +
                    "not the save is successful, the creature cannot be the target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
