using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities; 
using BlueprintCore.Utils; 
using Kingmaker.Localization;
using Kingmaker.Utility;

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

        // --- Warning! Key ---
        private const string Key_Duration_6Rounds = "CO.Duration.6Rounds";
        private const string Key_Duration_4Rounds = "CO.Duration.4Rounds";
        private const string Key_Duration_3Rounds = "CO.Duration.3Rounds";
        private const string Key_Duration_2d4Rounds = "CO.Duration.2d4Rounds";
        private const string Key_Duration_2d3Rounds = "CO.Duration.2d3Rounds";
        private const string Key_Duration_1d4Rounds = "CO.Duration.1d4Rounds";
        private const string Key_Duration_1d3Rounds = "CO.Duration.1d3Rounds";
        private const string Key_Duration_1d2Rounds = "CO.Duration.1d2Rounds";

        private static LocalizedString LsFromKey(string key, string valueIfMissing)
        {
            var pack = LocalizationManager.CurrentPack;
            pack?.PutString(key, valueIfMissing); 
            return new LocalizedString { m_Key = key };
        }

        public static AbilityConfigurator SetDuration6RoundsShared(this AbilityConfigurator cfg)
        {
            return cfg.OnConfigure(bp =>
            {
                bp.LocalizedDuration = LsFromKey(Key_Duration_6Rounds, "6 rounds");
            });
        }
        public static AbilityConfigurator SetDuration4RoundsShared(this AbilityConfigurator cfg)
        {
            return cfg.OnConfigure(bp =>
            {
                bp.LocalizedDuration = LsFromKey(Key_Duration_4Rounds, "4 rounds");
            });
        }
        public static AbilityConfigurator SetDuration3RoundsShared(this AbilityConfigurator cfg)
        {
            return cfg.OnConfigure(bp =>
            {
                bp.LocalizedDuration = LsFromKey(Key_Duration_3Rounds, "3 rounds");
            });
        }
        public static AbilityConfigurator SetDuration2d4RoundsShared(this AbilityConfigurator cfg)
        {
            return cfg.OnConfigure(bp =>
            {
                bp.LocalizedDuration = LsFromKey(Key_Duration_2d4Rounds, "2d4 rounds");
            });
        }
        public static AbilityConfigurator SetDuration2d3RoundsShared(this AbilityConfigurator cfg)
        {
            return cfg.OnConfigure(bp =>
            {
                bp.LocalizedDuration = LsFromKey(Key_Duration_2d3Rounds, "2d3 rounds");
            });
        }
        public static AbilityConfigurator SetDuration1d4RoundsShared(this AbilityConfigurator cfg)
        {
            return cfg.OnConfigure(bp =>
            {
                bp.LocalizedDuration = LsFromKey(Key_Duration_1d4Rounds, "1d4 rounds");
            });
        }
        public static AbilityConfigurator SetDuration1d3RoundsShared(this AbilityConfigurator cfg)
        {
            return cfg.OnConfigure(bp =>
            {
                bp.LocalizedDuration = LsFromKey(Key_Duration_1d3Rounds, "1d3 rounds");
            });
        }
        public static AbilityConfigurator SetDuration1d2RoundsShared(this AbilityConfigurator cfg)
        {
            return cfg.OnConfigure(bp =>
            {
                bp.LocalizedDuration = LsFromKey(Key_Duration_1d2Rounds, "1d2 rounds");
            });
        }

    }
}
