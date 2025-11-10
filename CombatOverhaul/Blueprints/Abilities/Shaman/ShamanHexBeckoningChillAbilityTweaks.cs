using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexBeckoningChillAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexBeckoningChillAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman causes one creature within 30 feet to become more susceptible to the sapping powers " +
                    "of cold for 1 minute. When a creature takes cold damage while under this effect, it is entangled " +
                    "for 1 round. If the creature takes cold damage while already entangled by beckoning chill, the " +
                    "duration of the entangled condition increases by 1 round. Once affected, the creature cannot be " +
                    "the target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
