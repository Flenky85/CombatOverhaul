using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexFortuneAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexFortuneAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman can grant a creature within 30 feet a bit of good luck for 1 round. The target can call upon this good luck, " +
                    "allowing him to reroll any ability check, attack roll, saving throw, or skill check, taking the better result. At 8th " +
                    "level and 16th level, the duration of this hex is extended by 1 round. Once a creature has benefited from the fortune hex, " +
                    "it cannot benefit from it again on new combat."
                )
                .Configure();
        }
    }
}
