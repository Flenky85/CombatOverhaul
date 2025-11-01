using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class StigmataFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.Stigmata)
                .SetDescriptionValue(
                    "As a standard action, the martyr can chant hymns of faith and cause bleeding stigmata " +
                    "to visibly appear on his body; at 7th level, he can manifest stigmata as a move action, " +
                    "and at 13th level, he can do so as a swift action. While his stigmata are active, he " +
                    "takes 1 point of bleed damage, which automatically ceases when he ends this ability but " +
                    "otherwise does not relent, even in the face of magical healing or Heal checks. His " +
                    "stigmata assist his allies, duplicating inspire courage bardic performance of a bard of " +
                    "his paladin level. At 10th level, he can choose to duplicate the effects of inspire " +
                    "greatness. At 16th level, he can choose to duplicate the effects of inspire heroics.\n" +
                    "hymns of faith uses charges; activating this ability expends 1 charges per round. The paladin begins with 3 " +
                    "charges; she also adds her Cha bonus (if any) to her maximum number of charges. At the start of her turn, if this ability " +
                    "is not active that round, the paladin regains 1 charges, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
