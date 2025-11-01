using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class ChannelEnergyHospitalerResourcesTweaks
    {
        public static void Register()
        {
            var res = BlueprintTool.Get<BlueprintAbilityResource>(AbilitiesResourcesGuids.ChannelEnergyHospitaler);
            var amt = res.m_MaxAmount;   
            amt.BaseValue = 12;          

            AbilityResourceConfigurator.For(AbilitiesResourcesGuids.ChannelEnergyHospitaler)
              .SetMaxAmount(amt)         
              .Configure();
        }
    }
}
