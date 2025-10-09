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
    internal static class ImprovedTwoWeaponFighting
    {
        private static bool _done;
        private const string ImprovedTWF_Guid = "9af88f3ed8a017b45a6837eab7437629";

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(ImprovedTWF_Guid);
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
                var descKey = feat.m_Description?.m_Key;
                if (!string.IsNullOrEmpty(descKey))
                    pack.PutString(descKey, "Increases damage dealt by 2.5% per point of Dexterity bonus when using finesse weapons only.");

                var shortKey = feat.m_DescriptionShort?.m_Key;
                if (!string.IsNullOrEmpty(shortKey))
                    pack.PutString(shortKey, "+2.5% damage per point of DEX bonus with finesse weapons only.");
            }
        }
    }
}
