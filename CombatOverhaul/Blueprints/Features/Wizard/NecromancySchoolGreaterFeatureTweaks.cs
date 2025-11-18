using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Wizard
{
    [AutoRegister]
    internal class NecromancySchoolGreaterFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.NecromancySchoolGreaterFeature)
                .SetDescriptionValue(
                    "At 8th level, you gain blindsight to a range of 10 feet. This ability only allows you to detect living creatures and undead " +
                    "creatures. This sight also tells you whether a creature is living or undead. Constructs and other creatures that are neither " +
                    "living nor undead cannot be seen with this ability. The range of this ability increases by 10 feet at 12th level, and by an " +
                    "additional 10 feet for every four levels beyond 12th." +
                    "Change shape starts with 3 charges and gains 1 additional charge every 5 levels; you can spend 1 charge for each round " +
                    "the ability is active, and you regain 1 charge each round while the ability is not active."
                )
                .Configure();
        }
    }
}
