using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class RestorationGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.RestorationGreater)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetMaterialComponent(new BlueprintAbility.MaterialComponentData
                {
                    m_Item = null,
                    Count = 0
                })
                .Configure();
        }
    }
}
