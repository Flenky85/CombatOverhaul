using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexHealingAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexHealingAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "This acts as a cure light wounds spell, using the shaman's caster level. " +
                    "Once a creature has benefited from the healing hex, it cannot benefit from " +
                    "it again on new combat. At 5th level, this hex acts like cure moderate wounds."
                )
                .Configure();
        }
    }
}
