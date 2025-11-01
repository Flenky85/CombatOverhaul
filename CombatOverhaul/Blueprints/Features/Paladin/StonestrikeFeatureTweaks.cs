using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class StonestrikeFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.Stonestrike)
                .SetDescriptionValue(
                    "A stonelord can draw upon the power of the living rock. " +
                    "As a swift action, she treats her melee attacks until the beginning of her next turn " +
                    "(whether armed or unarmed) as magical and adamantine with a +1 bonus on attack and " +
                    "damage rolls. This bonus increases by +1 at 5th level and every 5 levels thereafter.\n" +
                    "Stonestrike uses charges; activating this ability expends 4 charges. The paladin begins with 4 " +
                    "charges, and and every level she gains 1 additional charge. " +
                    "At the start of each round, the paladin regains 1 charge, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
