using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Shaman
{
    [AutoRegister]
    internal class WitchDoctorChannelPositiveFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.WitchDoctorChannelPositiveFeature)
                .SetDescriptionValue(
                    "At 4th level, the witch doctor can draw transcendental energies to her location, flooding " +
                    "it with positive energy as the cleric class feature. The witch doctor uses her shaman level — 3 " +
                    "as her effective cleric level. This is a separate pool of channel energy that doesn't stack with " +
                    "the life spirit's channel spirit ability.\n" +
                    "Activating this ability expends 6 charges. The shaman has a number of charges equal to " +
                    "6 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
