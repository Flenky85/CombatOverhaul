using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class TouchOfGracelessnessCastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TouchOfGracelessnessCast)
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "A coruscating ray springs from your hand. You must succeed on a ranged touch attack to strike a target. " +
                    "On a hit, the target takes 1d4 points of negative energy damage per caster level, maximum 4d4. " +
                    "The target then attempts a Fortitude save; on a success, it takes half damage and the spell applies no Dexterity penalty. " +
                    "On a failure, the target takes full damage and also takes a penalty to Dexterity equal to 1d6 plus 1 per two caster levels, maximum 1d6+5. " +
                    "The subject's Dexterity score cannot drop below 1. This penalty does not stack with itself; apply the highest penalty instead. " +
                    "The Dexterity penalty lasts for 2d3 rounds."
                )
                .Configure();
        }
    }
}
