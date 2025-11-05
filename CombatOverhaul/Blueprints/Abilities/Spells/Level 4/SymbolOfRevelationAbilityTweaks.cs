using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;


namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class SymbolOfRevelationAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SymbolOfRevelation)
                .SetMaterialComponent(new BlueprintAbility.MaterialComponentData
                {
                    m_Item = null,
                    Count = 0
                })
                .Configure();

            
        }
    }
}
