using BlueprintCore.Utils; 
using Kingmaker.Blueprints.Classes;
using Kingmaker.Localization;

namespace CombatOverhaul.Utils
{
    internal static class DescriptionUtils
    {
        /// <summary>
        /// Actualiza description y shortDescription de un Feature sin cambiar las keys.
        /// </summary>
        public static void SetFeatDescription(
            BlueprintFeature feat,
            string text,
            string shortText = null,
            bool tagEncyclopedia = true)
        {
            if (feat == null) return;

            // Prepara textos
            string Process(string s) =>
                tagEncyclopedia ? EncyclopediaTool.TagEncyclopediaEntries(s) : s;

            var pack = LocalizationManager.CurrentPack;
            if (pack == null) return;

            // Descripción larga
            var descKey = feat.m_Description?.m_Key;
            if (!string.IsNullOrEmpty(descKey))
            {
                var value = Process(text);
                pack.PutString(descKey, value);
            }

            // Descripción corta
            var shortKey = feat.m_DescriptionShort?.m_Key;
            if (!string.IsNullOrEmpty(shortKey))
            {
                var value = Process(shortText ?? text);
                pack.PutString(shortKey, value);
            }
        }

        public static void SetDescription(
            this BlueprintFeature feat,
            string text,
            string shortText = null,
            bool tagEncyclopedia = true)
            => SetFeatDescription(feat, text, shortText, tagEncyclopedia);
    }
}
