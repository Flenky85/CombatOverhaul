using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Monk
{
    [AutoRegister]
    internal class SenseiMysticWisdomTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.SenseiMysticWisdom)
                .SetDescriptionValue(
                    "At 10th level, a sensei may spend 4 point from his ki pool (as a swift action) while using " +
                    "advice to provide a single ally within 30 feet with evasion, fast movement, high jump, purity of body, or slow fall.\n" +
                    "At 14th level, a sensei may spend 4 points to grant one of the abilities listed above to all allies within 30 feet, or " +
                    "provide improved evasion to a single ally within 30 feet. These abilities function at the sensei's level and last 6 rounds."
                )
                .Configure();
        }
    }
}
