using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    internal class SmiteEvilFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.SmiteEvil)
                .SetDescriptionValue(
                    "As a swift action, the paladin chooses one target within sight to smite. If this target is evil, " +
                    "the paladin adds her Cha bonus (if any) to her attack rolls and adds her paladin level to all " +
                    "damage rolls made against the target of her smite, smite evil attacks automatically bypass any DR the " +
                    "creature might possess.\n" +
                    "In addition, while smite evil is in effect, the paladin gains a deflection bonus " +
                    "equal to her Charisma modifier (if any) to her AC against attacks made by the target of the smite. If the " +
                    "paladin targets a creature that is not evil, the smite is wasted with no effect.\n" +
                    "Smite evil lasts until the target dies or the paladin selects a new target.\n" +
                    "Smite evil uses charges; activating this ability expends 3 charges. The paladin begins with 3 " +
                    "charges, and at 3rd level and every 3 levels thereafter she gains 1 additional charge. " +
                    "At the start of each round, the paladin regains 1 charge, up to her maximum number of charges."
                    )
                .Configure();
        }
    }
}
