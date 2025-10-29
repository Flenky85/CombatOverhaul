using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    internal static class LayOnHandsResourcesTweaks
    {
        public static void Register()
        {
            var res = ResourcesLibrary.TryGetBlueprint<BlueprintAbilityResource>(AbilitiesResourcesGuids.LayOnHands);
            if (res == null) return;

            var amt = res.m_MaxAmount;
            amt.BaseValue = 4;
            res.m_MaxAmount = amt;
        }
    }
}
