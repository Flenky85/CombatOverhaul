using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Hellknight
{
    [AutoRegister]
    internal static class HellknightCommandAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HellknightCommandAbility)
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "This spell functions like command, except this spell affects multiple enemies in a " +
                    "30-foot radius, and the activities continue beyond 1 round. At the start of each " +
                    "commanded creature's turn after the first, it gets another Will save to attempt to " +
                    "break free from the spell. Each creature must receive the same command\n." + 
                    "This ability has a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
