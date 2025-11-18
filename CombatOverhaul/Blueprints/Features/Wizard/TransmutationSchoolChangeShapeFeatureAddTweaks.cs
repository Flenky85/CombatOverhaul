using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Wizard
{
    [AutoRegister]
    internal class TransmutationSchoolChangeShapeFeatureAddTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.TransmutationSchoolChangeShapeFeatureAdd)
                .SetDescriptionValue(
                    "At 8th level, you can change your shape like beast shape II or elemental body I. " +
                    "At 12th level, this ability functions like beast shape III or elemental body II." +
                    "Change shape starts with 3 charges and gains 1 additional charge every 5 levels; you can spend 1 charge for each round " +
                    "the ability is active, and you regain 1 charge each round while the ability is not active."
                )
                .Configure();
        }
    }
}
