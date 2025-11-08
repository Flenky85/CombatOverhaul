using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class PrayerAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Prayer)
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "You bring special favor upon yourself and your allies while bringing disfavor to your enemies. " +
                    "You and each of your allies gain a +1 luck bonus on attack rolls, weapon damage rolls, saves, " +
                    "and skill checks, while each of your foes takes a –1 penalty on such rolls.\n" +
                    "The duration on the enemy is 2d3 rounds."
                )
                .Configure();
        }
    }
}
