using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Shaman
{
    [AutoRegister]
    internal static class ShamanHexAuraOfPurityResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.ShamanHexAuraOfPurityResource;
            var res = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = res.m_MaxAmount;

            amount.IncreasedByLevel = false;
            amount.IncreasedByLevelStartPlusDivStep = true;
            amount.StartingLevel = 0;     
            amount.StartingIncrease = 2;  
            amount.LevelStep = 5;         
            amount.PerStepIncrease = 1;   
            amount.m_ClassDiv = amount.m_Class;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)  
                .Configure();
        }
    }
}
