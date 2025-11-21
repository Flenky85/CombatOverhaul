using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Ranger
{
    [AutoRegister]
    internal static class QuarryAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.QuarryAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "A character can, as a swift action, denote one target within his line of sight as his quarry. " +
                    "He receives a +2 insight bonus on attack rolls made against his quarry, and all critical threats " +
                    "are automatically confirmed. A character can have no more than one quarry at a time."
                )
                .Configure();
        }
    }
}
