using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Wizard
{
    [AutoRegister]
    internal class ArcaneBombsAcidFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ArcaneBombsAcidFeature)
                .SetDescriptionValue(
                    "At 1st level, the arcane bomber gains an ability nearly identical to the alchemist's bomb ability. " +
                    "Unlike the alchemist, at 1st level, the arcane bomber chooses one type of energy from the following list: " +
                    "acid, cold, fire, and electricity. He can throw bombs of that type, but cannot modify them with discoveries.\n" +
                    "The ability have a cooldown of 3 rounds.\n" +
                    "Thrown bombs have a range of 30 feet and use the Throw Splash Weapon special attack.Bombs are considered weapons " +
                    "and can be selected using feats such as Point - Blank Shot and Weapon Focus. On a direct hit, an arcane bomber's " +
                    "bomb inflicts 1d6 points of damage of the chosen energy type + additional damage equal to the arcane bomber's " +
                    "Intelligence modifier.The damage of an arcane bomber's bomb increases by 1d6 points at every odd-numbered arcane " +
                    "bomber level. Splash damage from an arcane bomber's bomb is always equal to the bomb's minimum damage (so if the " +
                    "bomb would deal 2d6+4 points of damage of the chosen energy type on a direct hit, its splash damage would be 6 " +
                    "points of damage of chosen energy type). Those caught in the splash damage can attempt a Reflex save for half damage. " +
                    "The DC of this save is equal to 10 + 1/2 the arcane bomber's level +the arcane bomber's Intelligence modifier."
                )
                .Configure();
        }
    }
}
