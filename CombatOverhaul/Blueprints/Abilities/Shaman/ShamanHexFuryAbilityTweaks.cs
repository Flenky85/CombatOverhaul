using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexFuryAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexFuryAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "A shaman incites a creature within 30 feet into a primal fury. The target receives a " +
                    "+2 morale bonus on attack rolls and a +2 resistance bonus on saving throws against fear " +
                    "for a number of rounds equal to the shaman's Wisdom modifier. At 8th and 16th levels, these " +
                    "bonuses increase by 1. Once a creature has benefited from the fury hex, it cannot benefit " +
                    "from it again on new combat."
                )
                .Configure();
        }
    }
}
