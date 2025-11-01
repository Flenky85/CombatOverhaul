using BlueprintCore.Blueprints.CustomConfigurators;
using CombatOverhaul.Guids;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class InspiredCourageBuffTweaks
    {
        public static void Register()
        {
            AbilityResourceConfigurator.For(AbilitiesResourcesGuids.MartyrPerformance)
              .SetMax(0)
              .Configure();
        }
    }
}
