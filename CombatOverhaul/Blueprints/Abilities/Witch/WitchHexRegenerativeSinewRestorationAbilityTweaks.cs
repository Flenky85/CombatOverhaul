using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexRegenerativeSinewRestorationAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexRegenerativeSinewRestorationAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The target heals up to 4 points of ability score damage from two ability scores.\n" +
                    "Once a creature has benefited from this hex, it cannot benefit from it again on new combat."
                )
                .Configure();
        }
    }
}
