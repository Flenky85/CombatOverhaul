using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class AllIsDarknessFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.AllIsDarkness)
                .SetDescriptionValue(
                    "A tortured crusader can call out to the powers of good to aid her in her struggle " +
                    "against evil. As a swift action, the tortured crusader chooses one target within sight " +
                    "to smite. The tortured crusader adds half her Wisdom bonus (minimum 1) to her attack " +
                    "rolls and adds her paladin level to all damage rolls made against the target of her " +
                    "smite. Smite evil attacks automatically bypass any DR the creature might possess.\n" +
                    "In addition, while smite evil is in effect, the tortured crusader gains a + 4 deflection " +
                    "bonus to her AC against attacks made by the target of the smite. Smite evil lasts until " +
                    "the target dies or the tortured crusader selects a new target.\n" +
                    "Smite evil uses charges; activating this ability expends 3 charges. The paladin begins with 4 " +
                    "charges, and at every 5 levels thereafter she gains 1 additional charge. " +
                    "At the start of each round, the paladin regains 1 charge, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
