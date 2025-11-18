using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Witch
{
    [AutoRegister]
    internal class SunderHopeFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.SunderHopeFeature)
                .SetDescriptionValue(
                    "At 8th level, once per combat after a hag of Gyronna has successfully affected a creature with a " +
                    "mind-affecting spell, spell-like ability, or hex, she also affects that creature with a targeted " +
                    "greater dispel magic effect. At 14th and 20th levels, a hag of Gyronna can use this ability an " +
                    "additional time per combat."
                )
                .Configure();
        }
    }
}
