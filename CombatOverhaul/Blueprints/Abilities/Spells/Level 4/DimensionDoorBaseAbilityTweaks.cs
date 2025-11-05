using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class DimensionDoorBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DimensionDoorBase)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "You instantly transfer yourself from your current location to any other spot within " +
                    "range. If you teleport only yourself, this is a swift action. If you also bring " +
                    "friendly creatures within a 10-foot radius around you, this is a standard action."
                )
                .Configure();
        }
    }
}
