using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class SecondChanceFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.SecondChance)
                .SetDescriptionValue(
                    "At 2nd level, a tortured crusader can heal her wounds by touch as a swift action. " +
                    "With one use of this ability, a tortured crusader can heal " +
                    "1d6 hit points of damage for every two paladin levels she possesses.\n" +
                    "At 8th level, this healing increases to 1d8 hit points of damage for every two paladin levels.\n" +
                    "At 15th level, this healing is maximized as though affected by the Maximize Spell feat." +
                    "Lay on hands uses charges; activating this ability expends 3 charges. The paladin begins with 3 " +
                    "charges, and at 2nd level and every 2 levels thereafter she gains 1 additional charge; she also " +
                    "adds her Wisdom bonus (if any) to her maximum number of charges. At the start of each round, the " +
                    "paladin regains 1 charge, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
