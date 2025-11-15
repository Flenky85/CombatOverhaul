using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexRegenerativeSinewFastHealingAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexRegenerativeSinewFastHealingAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The target gains fast healing 5 for a number of rounds equal to half the witch's class level.\n" +
                    "Once a creature has benefited from this hex, it cannot benefit from it again on new combat."
                )
                .Configure();
        }
    }
}
