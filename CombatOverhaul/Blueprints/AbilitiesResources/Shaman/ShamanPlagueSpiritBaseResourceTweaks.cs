using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Shaman
{
    [AutoRegister]
    internal static class ShamanPlagueSpiritBaseResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.ShamanPlagueSpiritBaseResource;
            var resource = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = resource.m_MaxAmount;
            amount.BaseValue = 3;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
