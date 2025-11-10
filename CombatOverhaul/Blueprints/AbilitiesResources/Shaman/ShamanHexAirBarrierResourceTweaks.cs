using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Shaman
{
    [AutoRegister]
    internal static class ShamanHexAirBarrierResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.ShamanHexAirBarrierResource;
            var res = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = res.m_MaxAmount;

            amount.IncreasedByLevel = false;
            amount.IncreasedByLevelStartPlusDivStep = true;
            amount.StartingLevel = 0;     
            amount.StartingIncrease = 3;  
            amount.LevelStep = 10;         
            amount.PerStepIncrease = 3;   
            amount.m_ClassDiv = amount.m_Class;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)  
                .Configure();
        }
    }
}
