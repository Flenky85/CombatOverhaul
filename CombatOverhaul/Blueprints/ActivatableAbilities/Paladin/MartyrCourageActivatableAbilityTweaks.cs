/*using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Collections.Generic;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Paladin
{
    [AutoRegister]
    internal static class MartyrCourageActivatableAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.MartyrCourage)
                .EditComponent<ActivatableAbilityResourceLogic>(c =>
                {
                    c.SpendType = ActivatableAbilityResourceLogic.ResourceSpendType.NewRound; 
                    c.m_ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>
                    {
                    BlueprintTool.GetRef<BlueprintUnitFactReference>(BuffsGuids.MartyrCourage)
                    };
                })
                .Configure();
        }
    }
}*/
