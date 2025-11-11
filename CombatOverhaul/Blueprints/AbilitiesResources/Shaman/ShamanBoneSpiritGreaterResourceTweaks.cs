using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Shaman
{
    [AutoRegister]
    internal static class ShamanBoneSpiritGreaterResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.ShamanBoneSpiritGreaterResource;
            var resource = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = resource.m_MaxAmount;
            amount.BaseValue = 5;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
