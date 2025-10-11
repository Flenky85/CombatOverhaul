using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using CombatOverhaul.Guids;

namespace CombatOverhaul.Patches.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class DragonStyle
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.DragonStyle);
            if (feat == null) return;

            var pack = LocalizationManager.CurrentPack;
            if (pack == null) return;

            var enText =
                "You call upon the spirit of dragonkind, gaining greater resilience, mobility, and fierceness from their blessing.\n" +
                "While using this style, you gain a +2 bonus on saving throws against sleep effects, paralysis effects, and stunning effects. " +
                "Further, you add +5% damage per point of your Strength bonus to the damage roll of your first unarmed strike each round.";

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
