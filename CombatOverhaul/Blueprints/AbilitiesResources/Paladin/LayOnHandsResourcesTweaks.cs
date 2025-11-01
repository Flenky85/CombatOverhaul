using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class LayOnHandsResourcesTweaks
    {
        public static void Register()
        {
            var res = BlueprintTool.Get<BlueprintAbilityResource>(AbilitiesResourcesGuids.LayOnHands);
            var amt = res.m_MaxAmount;
            amt.BaseValue = 4;

            AbilityResourceConfigurator.For(AbilitiesResourcesGuids.LayOnHands)
              .SetMaxAmount(amt)
              .Configure();
        }
    }
}
