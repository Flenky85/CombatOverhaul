using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Hellknight
{
    [AutoRegister]
    internal static class HellknightDisciplineOnslaughtResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.HellknightDisciplineOnslaughtResource;
            var res = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = res.m_MaxAmount;

            amount.BaseValue = 3;
            amount.IncreasedByLevel = false;
            amount.IncreasedByLevelStartPlusDivStep = false;
            amount.IncreasedByStat = false;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
