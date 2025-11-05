using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Others
{
    [AutoRegister]
    internal static class TurnBackAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TurnBack)
                .SetActionType(UnitCommand.CommandType.Swift)
                .Configure();
        }
    }
}
