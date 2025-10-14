using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class CrushingBlow
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            FeatureConfigurator.For(FeaturesGuids.CrushingBlow)
                .SetDescriptionValue(
                    "You can make a Stunning Fist attempt as an action. If successful, instead of stunning your target, " +
                    "you reduce the target’s AC by an amount equal to your Wisdom modifier for 1 minute. This penalty " +
                    "doesn’t stack with other penalties applied due to Crushing Blow.")
                .Configure();
        }
    }
}
