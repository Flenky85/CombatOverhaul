using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Wizard
{
    [AutoRegister]
    internal static class UniversalistSchoolGreaterResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.UniversalistSchoolGreaterResource;
            var resource = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = resource.m_MaxAmount;

            amount.StartingLevel = 0;       
            amount.StartingIncrease = 0;
            amount.LevelStep = 2;           
            amount.PerStepIncrease = 1;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
