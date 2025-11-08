using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class RemoveDiseaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.RemoveDisease)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .Configure();
        }
    }
}
