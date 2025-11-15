using BlueprintCore.Blueprints.CustomConfigurators;
using CombatOverhaul.Guids;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Monk 
{ 
    [AutoRegister] 
    internal static class SenseiPerformanceResourceTweaks
    { 
        public static void Register() 
        {
            AbilityResourceConfigurator.For(AbilitiesResourcesGuids.SenseiPerformanceResource)
                .ModifyMaxAmount(a =>
                {
                    a.BaseValue = 2;
                    a.IncreasedByLevel = false;
                    a.IncreasedByLevelStartPlusDivStep = true;
                    a.StartingLevel = 0;
                    a.StartingIncrease = 0;
                    a.LevelStep = 4;
                    a.PerStepIncrease = 1;
                    a.IncreasedByStat = false;
                })
                .Configure();
        } 
    } 
}
