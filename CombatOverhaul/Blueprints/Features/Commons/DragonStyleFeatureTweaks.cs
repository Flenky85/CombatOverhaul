using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    [AutoRegister]
    internal static class DragonStyleFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.DragonStyle)
                .SetDescriptionValue(
                    "You call upon the spirit of dragonkind, gaining greater resilience, mobility, and fierceness from their blessing.\n" +
                        "While using this style, you gain a +2 bonus on saving throws against sleep effects, paralysis effects, and stunning effects. " +
                        "Further, you add +5% damage per point of your Strength bonus to the damage roll of your first unarmed strike each round.")
                .Configure();
        }
    }
}
