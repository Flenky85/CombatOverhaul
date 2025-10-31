using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class DivineGuardianTrothResourcesTweaks
    {
        public static void Register()
        {
            var res = ResourcesLibrary.TryGetBlueprint<BlueprintAbilityResource>(AbilitiesResourcesGuids.DivineGuardianTroth);
            if (res == null) return;

            var amt = res.m_MaxAmount;
            amt.BaseValue = 3;
            res.m_MaxAmount = amt;
        }
    }
}
