using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class DeathWardCastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DeathWardCast)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
