using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class ShiningLightResourcesTweaks
    {
        public static void Register()
        {
            var res = BlueprintTool.Get<BlueprintAbilityResource>(AbilitiesResourcesGuids.ShiningLight);
            var amt = res.m_MaxAmount;
            amt.BaseValue = 6;

            AbilityResourceConfigurator.For(AbilitiesResourcesGuids.ShiningLight)
              .SetMaxAmount(amt)
              .Configure();
        }
    }
}
