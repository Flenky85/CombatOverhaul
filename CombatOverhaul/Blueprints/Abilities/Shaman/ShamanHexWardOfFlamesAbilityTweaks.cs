using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexWardOfFlamesAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexWardOfFlamesAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman touches a willing creature (including herself) and grants a ward of flames. " +
                    "The next time the warded creature is struck with a melee attack, the creature making the " +
                    "attack takes 1d6 points of fire damage + 1 point of fire damage for every 2 shaman levels " +
                    "she possesses. This ward lasts for 1 minute, after which it fades away if not already expended. " +
                    "At 8th and 16th levels, the ward lasts for one additional attack. A creature affected by this " +
                    "hex cannot be affected by it again on new combat."
                )
                .Configure();
        }
    }
}
