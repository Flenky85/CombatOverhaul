using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    [AutoRegister]
    internal static class SunderArmorFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.SunderArmor)
                .SetDescriptionValue(
                    "You can attempt to dislodge a piece of armor worn by your opponent. If your combat maneuver is successful, " +
                    "the target loses its bonuses from armor for 1 round.\n" +
                    "For every 5 by which your attack exceeds your opponent's CMD, the penalty lasts 1 additional round.\n" +
                    "In addition, while this effect lasts, the target’s damage reduction from armor is halved. This reduction also applies to monsters.")
                .Configure();
        }
    }
}
