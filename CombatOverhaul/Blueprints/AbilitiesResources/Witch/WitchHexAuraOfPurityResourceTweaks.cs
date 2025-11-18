using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Witch
{
    [AutoRegister]
    internal static class WitchHexAuraOfPurityResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.WitchHexAuraOfPurityResource;
            var res = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = res.m_MaxAmount;

            amount.StartingLevel = 0;     
            amount.StartingIncrease = 2;  
            amount.LevelStep = 5;         
            amount.PerStepIncrease = 1;   

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)  
                .Configure();
        }
    }
}
