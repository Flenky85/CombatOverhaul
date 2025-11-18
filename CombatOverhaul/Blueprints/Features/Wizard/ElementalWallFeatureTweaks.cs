using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Wizard
{
    [AutoRegister]
    internal class ElementalWallFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ElementalWallFeature)
                .SetDescriptionValue(
                    "At 8th level, once per day you can create a wall of energy. You can choose fire, cold, acid, or electricity damage.\n" +
                    "An immobile, blazing curtain of the chosen energy springs into existence. The wall deals damage of the chosen energy " +
                    "type when it appears, and to all creatures in the area on your turn each round. The wall also deals 2d6 points of " +
                    "damage of the chosen energy type + 1 point of that type per caster level(maximum + 20) to any creature passing through " +
                    "it. The wall deals double damage to undead creatures.\n" +
                    "The ability have a cooldown of 4 rounds."
                )
                .Configure();
        }
    }
}
