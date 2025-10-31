using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class ChannelEnergyHospitalerResourcesTweaks
    {
        public static void Register()
        {
            var res = ResourcesLibrary.TryGetBlueprint<BlueprintAbilityResource>(AbilitiesResourcesGuids.ChannelEnergyHospitaler);
            if (res == null) return;

            var amt = res.m_MaxAmount;
            amt.BaseValue = 12;
            res.m_MaxAmount = amt;
        }
    }
}
