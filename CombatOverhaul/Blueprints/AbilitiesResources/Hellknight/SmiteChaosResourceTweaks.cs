using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Hellknight
{
    [AutoRegister]
    internal static class SmiteChaosResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.SmiteChaosResource;
            var resource = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = resource.m_MaxAmount;
            amount.BaseValue = 3;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
