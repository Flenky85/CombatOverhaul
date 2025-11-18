using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Wizard
{
    [AutoRegister]
    internal class UniversalistSchoolExtendReachFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.UniversalistSchoolExtendReachFeature)
                .SetDescriptionValue(
                    "At 8th level, you can extend or alter the range of your next spell as though using the Extend Spell or Reach Spell feat accordingly, " +
                    "Extend and reach costs 2 charges of metamagic mastery."
                )
                .Configure();
        }
    }
}
