using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class SmiteEvilResourcesTweaks
    {
        public static void Register()
        {
            var res = BlueprintTool.Get<BlueprintAbilityResource>(AbilitiesResourcesGuids.SmiteEvil);
            var amt = res.m_MaxAmount;
            amt.BaseValue = 3;

            AbilityResourceConfigurator.For(AbilitiesResourcesGuids.SmiteEvil)
              .SetMaxAmount(amt)
              .Configure();
        }
    }
}
