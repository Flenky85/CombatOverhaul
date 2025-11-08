using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class RestorationAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Restoration)
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
