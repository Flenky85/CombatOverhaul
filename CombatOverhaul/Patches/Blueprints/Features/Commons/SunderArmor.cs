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
    internal static class SunderArmor
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.SunderArmor);
            if (feat == null) return;

            var pack = LocalizationManager.CurrentPack;
            if (pack == null) return;

            var enText =
                "You can attempt to dislodge a piece of armor worn by your opponent. If your combat maneuver is successful, " +
                "the target loses its bonuses from armor for 1 round.\n" +
                "For every 5 by which your attack exceeds your opponent's CMD, the penalty lasts 1 additional round.\n" +
                "In addition, while this effect lasts, the target’s damage " +
                "reduction from armor is halved. This reduction also applies to monsters.";

            feat.SetDescription(enText);
        }
    }
}
