using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Monk
{
    [AutoRegister]
    internal static class StunningFistResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.StunningFistResource;
            var res = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = res.m_MaxAmount;

            amount.IncreasedByLevel = false;
            amount.IncreasedByLevelStartPlusDivStep = true; 
            amount.StartingLevel = 0;
            amount.StartingIncrease = 3; 
            amount.LevelStep = 5;        
            amount.PerStepIncrease = 1;
            amount.OtherClassesModifier = 1f;
            amount.IncreasedByStat = false;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
