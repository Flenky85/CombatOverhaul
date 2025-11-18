using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Wizard
{
    [AutoRegister]
    internal static class IllusionSchoolGreaterResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.IllusionSchoolGreaterResource;
            var resource = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var cls = BlueprintTool.GetRef<BlueprintCharacterClassReference>(ClassGuids.WizardClass);

            var amount = resource.m_MaxAmount;

            amount.BaseValue = 2;
            amount.IncreasedByLevel = false;
            amount.LevelIncrease = 0;

            amount.IncreasedByLevelStartPlusDivStep = true;
            amount.StartingLevel = 0;
            amount.StartingIncrease = 1;
            amount.LevelStep = 5;
            amount.PerStepIncrease = 1;
            amount.MinClassLevelIncrease = 0;
            amount.m_ClassDiv = new[] { cls };

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
