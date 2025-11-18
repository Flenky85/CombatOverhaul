using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Wizard
{
    [AutoRegister]
    internal class AbjurationResistanceFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.AbjurationResistanceFeature)
                .SetDescriptionValue(
                    "You gain resistance 5 to an energy type of your choice, chosen when you prepare spells. " +
                    "This resistance can be changed each combat. At 11th level, this resistance increases to 10. " +
                    "At 20th level, this resistance changes to immunity to the chosen energy type."
                )
                .Configure();
        }
    }
}
