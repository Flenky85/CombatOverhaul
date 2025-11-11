using Kingmaker.Localization;

namespace CombatOverhaul.utils
{
    public static class LocalizationUtils
    {
        private const string Prefix = "CO"; 

        public static LocalizedString CreateString(string key, string text)
        {
            var ls = new LocalizedString { m_Key = key };
            LocalizationManager.CurrentPack.PutString(key, text);
            return ls;
        }

        public static LocalizedString MakeName(string assetId, string text)
            => CreateString($"{Prefix}_{assetId}_Name", text);

        public static LocalizedString MakeDescription(string assetId, string text)
            => CreateString($"{Prefix}_{assetId}_Desc", text);
    }
}
