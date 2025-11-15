using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexRegenerativeSinewAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexRegenerativeSinewAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The witch can cause the debilitating wounds of a creature she touches to quickly close, helping it heal rapidly.\n" +
                    "The target gains fast healing 5 for a number of rounds equal to 1 / 2 the witch's class level or it heals up to 4 points " +
                    "of ability score damage from two ability scores.\n" +
                    "Once a creature has benefited from this hex, it cannot benefit from it again on new combat."
                )
                .Configure();
        }
    }
}
