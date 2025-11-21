using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Cleric
{
    [AutoRegister]
    internal static class TravelDomainGreaterResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.TravelDomainGreaterResource;
            var resource = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = resource.m_MaxAmount;
            
            amount.BaseValue = 6;
            amount.IncreasedByLevel = false;
            amount.IncreasedByLevelStartPlusDivStep = false;
            amount.m_Class = System.Array.Empty<BlueprintCharacterClassReference>();
            amount.m_ClassDiv = System.Array.Empty<BlueprintCharacterClassReference>();
            amount.LevelIncrease = 0;
            amount.StartingLevel = 0;
            amount.StartingIncrease = 0;
            amount.LevelStep = 0;
            amount.PerStepIncrease = 0;
            amount.IncreasedByStat = false;

            AbilityResourceConfigurator.For(guid)
              .SetMaxAmount(amount)
              .Configure();
        }
    }
}
