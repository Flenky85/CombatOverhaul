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
    internal static class GreaterTwoWeaponFighting
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.GreaterTwoWeaponFighting);
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
                var enText = "Upgrades Improved Two-Weapon Fighting: your off-hand attacks with finesse weapons gain +5% damage per point of Dexterity bonus.";

                enText = BlueprintCore.Utils.EncyclopediaTool.TagEncyclopediaEntries(enText);

                var descKey = feat.m_Description?.m_Key;
                if (!string.IsNullOrEmpty(descKey))
                    pack.PutString(descKey, enText);

                var shortKey = feat.m_DescriptionShort?.m_Key;
                if (!string.IsNullOrEmpty(shortKey))
                    pack.PutString(shortKey, enText);
            }
        }
    }
}
