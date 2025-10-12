using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Patches.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class CrushingBlow
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.CrushingBlow);
            if (feat == null) return;

            var pack = LocalizationManager.CurrentPack;
            if (pack == null) return;

            var enText =
                "You can make a Stunning Fist attempt as an action. If successful, instead of stunning your target, " +
                "you reduce the target’s AC by an amount equal to your Wisdom modifier for 1 minute. This penalty " +
                "doesn’t stack with other penalties applied due to Crushing Blow.";

            feat.SetDescription(enText);
        }
    }
}
