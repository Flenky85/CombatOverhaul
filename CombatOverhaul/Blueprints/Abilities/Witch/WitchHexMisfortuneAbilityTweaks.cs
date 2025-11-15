using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexMisfortuneAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexMisfortuneAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The witch can cause a creature within 30 feet to suffer grave misfortune for 1 round. " +
                    "Anytime the creature makes an ability check, attack roll, saving throw, or skill check, " +
                    "it must roll twice and take the worse result. A Will save negates this hex. At 8th level " +
                    "and 16th level, the duration of this hex is extended by 1 round. This hex affects all rolls " +
                    "the target must make while it lasts. Whether or not the save is successful, a creature cannot " +
                    "be the target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
