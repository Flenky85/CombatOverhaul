using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Monk
{
    [AutoRegister]
    internal static class PerfectStrikeResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.PerfectStrikeResource;
            var res = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amt = res.m_MaxAmount;

            amt.BaseValue = 3;
            amt.IncreasedByLevel = false;
            amt.IncreasedByLevelStartPlusDivStep = false;
            amt.IncreasedByStat = false;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amt)
                .Configure();
        }
    }
}
