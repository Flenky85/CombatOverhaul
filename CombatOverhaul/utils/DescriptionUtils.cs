using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities; 
using BlueprintCore.Utils; 
using Kingmaker.Localization;

namespace CombatOverhaul.Utils
{
    internal static class BlueprintCoreDescriptionExtensions
    {
        private static string ResolveLocalizedKey(LocalizedString ls)
        {
            if (ls == null) return string.Empty;
            var cur = ls; int guard = 0;
            while (cur.Shared != null && guard < 50)
            {
                guard++;
                cur = cur.Shared.String;
                if (cur == null) break;
            }
            return cur?.Key ?? string.Empty;
        }

        private static void PutResolved(LocalizedString ls, string value)
        {
            var pack = LocalizationManager.CurrentPack;
            if (ls == null || pack == null) return;

            var key = ResolveLocalizedKey(ls);
            if (!string.IsNullOrEmpty(key))
                pack.PutString(key, value);
        }

        private static string Process(string s, bool tagEncyclopedia) =>
            tagEncyclopedia ? EncyclopediaTool.TagEncyclopediaEntries(s) : s;

        public static FeatureConfigurator SetDescriptionValue(
            this FeatureConfigurator cfg, string text, bool tagEncyclopedia = true)
        {
            return cfg.OnConfigure(bp =>
            {
                var v = Process(text, tagEncyclopedia);
                PutResolved(bp.m_Description, v);
                PutResolved(bp.m_DescriptionShort, v); 
            });
        }

        public static AbilityConfigurator SetDescriptionValue(
            this AbilityConfigurator cfg, string text, bool tagEncyclopedia = true)
        {
            return cfg.OnConfigure(bp =>
            {
                var v = Process(text, tagEncyclopedia);
                PutResolved(bp.m_Description, v);
                PutResolved(bp.m_DescriptionShort, v);
            });
        }

        public static FeatureSelectionConfigurator SetDescriptionValue(
            this FeatureSelectionConfigurator cfg, string text, bool tagEncyclopedia = true)
        {
            return cfg.OnConfigure(bp =>
            {
                var v = Process(text, tagEncyclopedia);
                PutResolved(bp.m_Description, v);
                PutResolved(bp.m_DescriptionShort, v);
            });
        }
        public static ActivatableAbilityConfigurator SetDescriptionValue(
            this ActivatableAbilityConfigurator cfg, string text, bool tagEncyclopedia = true)
        {
            return cfg.OnConfigure(bp =>
            {
                var v = Process(text, tagEncyclopedia);
                PutResolved(bp.m_Description, v);
                PutResolved(bp.m_DescriptionShort, v);
            });
        }

        public static AbilityConfigurator SetDurationValue(
            this AbilityConfigurator cfg, string text, bool tagEncyclopedia = false)
        {
            return cfg.OnConfigure(bp =>
            {
                var v = Process(text, tagEncyclopedia);
                PutResolved(bp.LocalizedDuration, v);
            });
        }
        public static AbilityConfigurator SetSavingThrowValue(
            this AbilityConfigurator cfg, string text, bool tagEncyclopedia = false)
        {
            return cfg.OnConfigure(bp =>
            {
                var v = Process(text, tagEncyclopedia);
                PutResolved(bp.LocalizedSavingThrow, v);
            });
        }
    }
}
