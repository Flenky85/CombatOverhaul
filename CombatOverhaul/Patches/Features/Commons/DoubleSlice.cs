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
        private const string DoubleSliceGuid = "8a6a1920019c45d40b4561f05dcb3240";

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(DoubleSliceGuid);
            if (feat == null) return;

            var comps = new List<BlueprintComponent>(feat.ComponentsArray);

            // Quitar el efecto original (STR en off-hand)
            for (int i = comps.Count - 1; i >= 0; i--)
            {
                if (comps[i] is AddMechanicsFeature amf &&
                    amf.m_Feature == AddMechanicsFeature.MechanicsFeatureType.DoubleSlice)
                {
                    comps.RemoveAt(i);
                }
            }

            feat.ComponentsArray = comps.ToArray();

            // Reutilizar keys existentes de localización (no crear nuevas)
            var pack = LocalizationManager.CurrentPack;
            if (pack != null)
            {
                var enText =
                    "<b>Off-hand only.</b> Your off-hand attacks gain <b>+4%</b> damage per point of <b>Strength bonus</b>.";

                var descKey = feat.m_Description?.m_Key;
                if (!string.IsNullOrEmpty(descKey))
                    pack.PutString(descKey, enText);

                var shortKey = feat.m_DescriptionShort?.m_Key;
                if (!string.IsNullOrEmpty(shortKey))
                    pack.PutString(shortKey, "+4% damage per point of STR bonus on off-hand attacks.");
            }
        }
    }
}
