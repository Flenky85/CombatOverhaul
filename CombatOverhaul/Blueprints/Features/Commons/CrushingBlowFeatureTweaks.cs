using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    internal static class CrushingBlowFeatureTweaks
    {
        public static void Register()
        {
            var id = FeaturesGuids.CrushingBlow;

            // Seguridad por si el feature no existe en esta build
            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(id);
            if (feat == null) return;

            const string desc =
                "You can make a Stunning Fist attempt as an action. If successful, instead of stunning your target, " +
                "you reduce the target’s AC by an amount equal to your Wisdom modifier for 1 minute. This penalty " +
                "doesn’t stack with other penalties applied due to Crushing Blow.";

            FeatureConfigurator.For(id)
                .SetDescriptionValue(desc)
                .Configure();
        }
    }
}
