using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.Mechanics.Buffs;

namespace CombatOverhaul.Blueprints.Features.Oracle
{
    [AutoRegister]
    internal static class LameCurseFeatureLevel1Tweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.LameCurseFeatureLevel1)
                .EditComponent<BuffMovementSpeed>(c =>
                {
                    c.Value = -5;
                })
                .SetDescriptionValue(
                "One of your legs is permanently wounded, reducing your base land speed by 5 feet. Your speed is never reduced due to encumbrance.\n" +
                "At 5th level, you are immune to the fatigued condition (but not exhaustion).\n" +
                "At 10th level, your speed is never reduced by armor.\n" +
                "At 15th level, you are immune to the exhausted condition."
                )
                .Configure();
        }
    }
}
