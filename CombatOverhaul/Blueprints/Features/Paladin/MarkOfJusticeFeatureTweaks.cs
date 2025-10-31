using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class MarkOfJusticeFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.MarkOfJustice)
                .SetDescriptionValue(
                    "At 11th level, a paladin can expend 6 charges of her smite evil ability to " +
                    "grant the ability to smite evil to all allies for 3 rounds, using her bonuses. " +
                    "As a swift action, the paladin chooses one target within sight to smite. If this " +
                    "target is evil, the paladin's allies add her Charisma bonus (if any) to their " +
                    "attack rolls and add her paladin level to all damage rolls made against the target " +
                    "of her smite. Smite evil attacks automatically bypass any DR the creature might possess.\n" +
                    "In addition, while smite evil is in effect, the paladin's allies gain a deflection bonus " +
                    "equal to her Charisma bonus (if any) to their AC against attacks made by the target of " +
                    "this smite. If the paladin targets a creature that is not evil, this smite is wasted " +
                    "with no effect."
                )
                .Configure();
        }
    }
}
