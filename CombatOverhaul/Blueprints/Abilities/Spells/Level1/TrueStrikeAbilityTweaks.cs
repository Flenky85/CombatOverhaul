using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level1
{
    [AutoRegister]
    internal static class TrueStrikeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TrueStrike)
                .SetActionType(UnitCommand.CommandType.Swift)  
                .SetIsFullRoundAction(false)
                .Configure();
        }
    }
}
