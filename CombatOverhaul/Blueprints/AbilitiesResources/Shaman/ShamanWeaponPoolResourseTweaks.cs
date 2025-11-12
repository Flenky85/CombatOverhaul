using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Shaman
{
    [AutoRegister]
    internal static class ShamanWeaponPoolResourseTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.ShamanWeaponPoolResourse;
            var res = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = res.m_MaxAmount;

            amount.IncreasedByLevel = false;
            amount.IncreasedByLevelStartPlusDivStep = true;
            amount.StartingLevel = 0;     
            amount.StartingIncrease = 3;  
            amount.LevelStep = 10;         
            amount.PerStepIncrease = 3;   
            amount.IncreasedByStat = false;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)  
                .Configure();
        }
    }
}
