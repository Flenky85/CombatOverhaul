using CombatOverhaul.Guids;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;

namespace CombatOverhaul.Patches.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class DoubleSlice
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.DoubleSlice);
            if (feat == null) return;

            var comps = new List<BlueprintComponent>(feat.ComponentsArray);

            for (int i = comps.Count - 1; i >= 0; i--)
            {
                if (comps[i] is AddMechanicsFeature amf &&
                    amf.m_Feature == AddMechanicsFeature.MechanicsFeatureType.DoubleSlice)
                {
                    comps.RemoveAt(i);
                }
            }

            feat.ComponentsArray = comps.ToArray();

            var pack = LocalizationManager.CurrentPack;
            if (pack != null)
            {
                var enText =
                    "Off-hand only. Your off-hand attacks with non-finesse weapons gain +5% damage per point of Strength bonus.";

                var descKey = feat.m_Description?.m_Key;
                if (!string.IsNullOrEmpty(descKey))
                    pack.PutString(descKey, enText);

                var shortKey = feat.m_DescriptionShort?.m_Key;
                if (!string.IsNullOrEmpty(shortKey))
                    pack.PutString(shortKey, "+5% damage per point of STR bonus on off-hand attacks with non-finesse weapons.");
            }
        }
    }
}
