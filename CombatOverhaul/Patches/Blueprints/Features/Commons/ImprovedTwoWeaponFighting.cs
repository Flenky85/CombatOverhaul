using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;

namespace CombatOverhaul.Patches.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class ImprovedTwoWeaponFighting
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.ImprovedTwoWeaponFighting);
            if (feat == null) return;

            var comps = new List<BlueprintComponent>(feat.ComponentsArray);

            for (int i = comps.Count - 1; i >= 0; i--)
            {
                if (comps[i] is AddFacts)
                    comps.RemoveAt(i);
            }

            feat.ComponentsArray = comps.ToArray();

            var pack = LocalizationManager.CurrentPack;
            if (pack != null)
            {
                var enText = "Increases damage convert 2.5% extra damage per point of Strength bonus into " +
                    "2.5% extra damage per point of Dexterity bonus. on off-hand attacks when using finesse weapons only.";

                feat.SetDescription(enText);
            }
        }
    }
}
