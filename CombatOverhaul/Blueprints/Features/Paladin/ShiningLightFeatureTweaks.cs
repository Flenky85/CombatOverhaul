using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class ShiningLightFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ShiningLight)
                .SetDescriptionValue(
                    "At 14th level, a warrior of the holy light can unleash a 30-foot burst of pure, white light " +
                    "as a standard action. Evil creatures within this burst take (1d6 for every two paladin levels) " +
                    "points of damage and are blinded for 1 round. Evil dragons, evil outsiders, and evil undead are " +
                    "blinded for 1d4 rounds on a failed save. A Reflex save halves this damage and negates the blindness. " +
                    "The DC of this save is equal to 10 + 1/2 the warrior of the holy light's level + the warrior of the " +
                    "holy light's Charisma modifier. Good creatures within this burst are healed 1d6 points of damage per " +
                    "two paladin levels and receive a +2 sacred bonus on ability checks, attack rolls, saving throws, and " +
                    "skill checks for 1 round.\n" +
                    "Shining light uses charges; activating this ability expends 6 charges. The paladin begins with 6 " +
                    "charges, at 17th level, she gains 3 additional charges, and 3 more at 20th level. " +
                    "At the start of each round, the paladin regains 1 charge, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
