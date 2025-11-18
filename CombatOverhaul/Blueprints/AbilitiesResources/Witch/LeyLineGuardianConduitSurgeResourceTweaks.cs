using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Witch
{
    [AutoRegister]
    internal static class LeyLineGuardianConduitSurgeResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.LeyLineGuardianConduitSurgeResource;
            var res = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var witch = BlueprintTool.GetRef<BlueprintCharacterClassReference>(ClassGuids.WitchClass);
            var amount = res.m_MaxAmount;

            amount.BaseValue = 3;
            amount.IncreasedByLevel = false;

            amount.IncreasedByLevelStartPlusDivStep = true;
            amount.StartingLevel = 10;
            amount.StartingIncrease = 3;
            amount.LevelStep = 10;
            amount.PerStepIncrease = 3;
            amount.MinClassLevelIncrease = 0;
            amount.m_ClassDiv = new[] { witch };
            amount.IncreasedByStat = false;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
