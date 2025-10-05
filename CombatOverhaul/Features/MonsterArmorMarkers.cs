using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Localization; 

namespace CombatOverhaul.Features
{
    internal static class MonsterArmorMarkers
    {
        public static BlueprintFeatureReference MediumRef;
        public static BlueprintFeatureReference HeavyRef;

        public const string MediumGuid = "7f8d6b9c-4c3a-4b64-8f9d-6a8f7e2b2e91";
        public const string HeavyGuid = "e3d0a3a5-8b1e-4a9e-bf9c-2b6a7f2d9c44";

        private static bool _registered;

        private const string MediumName = "CO_MonsterArmor_Medium";
        private const string HeavyName = "CO_MonsterArmor_Heavy";

        private const string K_Med_Name = "CO.MonsterArmor.Medium.Name";
        private const string K_Med_Desc = "CO.MonsterArmor.Medium.Desc";
        private const string K_Hev_Name = "CO.MonsterArmor.Heavy.Name";
        private const string K_Hev_Desc = "CO.MonsterArmor.Heavy.Desc";

        public static void Register()
        {
            if (_registered) return;    
            _registered = true;

            LocalizedString medName = LocalizationTool.CreateString(K_Med_Name, "Medium Armor Monster", tagEncyclopediaEntries: false);
            LocalizedString medDesc = LocalizationTool.CreateString(K_Med_Desc, "Reduces physical damage taken by 20% and AC by 12%.", false);
            LocalizedString hevName = LocalizationTool.CreateString(K_Hev_Name, "Heavy Armor Monster", false);
            LocalizedString hevDesc = LocalizationTool.CreateString(K_Hev_Desc, "Reduces physical damage taken by 40% and AC by 24%.", false);

            var medium = FeatureConfigurator
                .New(MediumName, MediumGuid)
                .SetDisplayName(medName)
                .SetDescription(medDesc)
                .SetRanks(1)
                .SetIsClassFeature(true)     
                .SetHideInUI(false)          
                .SetHideNotAvailibleInUI(false)
                .Configure();

            var heavy = FeatureConfigurator
                .New(HeavyName, HeavyGuid)
                .SetDisplayName(hevName)
                .SetDescription(hevDesc)
                .SetRanks(1)
                .SetIsClassFeature(true)     
                .SetHideInUI(false)          
                .SetHideNotAvailibleInUI(false)
                .Configure();

            MediumRef = medium.ToReference<BlueprintFeatureReference>();
            HeavyRef = heavy.ToReference<BlueprintFeatureReference>();
        }
    }
}
