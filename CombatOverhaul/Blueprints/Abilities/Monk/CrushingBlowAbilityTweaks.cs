using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class CrushingBlowAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CrushingBlowAbility)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .Configure();
        }
    }
}
