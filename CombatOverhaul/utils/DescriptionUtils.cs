using Kingmaker.Blueprints.Classes;
using Kingmaker.Localization;

namespace CombatOverhaul.Utils
{
    internal static class DescriptionUtils
    {
        public static void SetFeatDescription(BlueprintFeature feat, string text, string shortText = null, bool tagEncyclopedia = true)
        {
            if (feat == null) return;

            var pack = LocalizationManager.CurrentPack;
            if (pack == null) return;

            string desc = tagEncyclopedia
                ? BlueprintCore.Utils.EncyclopediaTool.TagEncyclopediaEntries(text)
                : text;

            var descKey = feat.m_Description?.m_Key;
            if (!string.IsNullOrEmpty(descKey))
                pack.PutString(descKey, desc);

            var shortKey = feat.m_DescriptionShort?.m_Key;
            if (!string.IsNullOrEmpty(shortKey))
            {
                string s = shortText ?? desc;
                s = tagEncyclopedia ? BlueprintCore.Utils.EncyclopediaTool.TagEncyclopediaEntries(s) : s;
                pack.PutString(shortKey, s);
            }
        }
        /// <summary>
        /// Versión de extensión por ergonomía.
        /// </summary>
        public static void SetDescription(this BlueprintFeature feat, string text, string shortText = null, bool tagEncyclopedia = true)
            => SetFeatDescription(feat, text, shortText, tagEncyclopedia);
    }
}
