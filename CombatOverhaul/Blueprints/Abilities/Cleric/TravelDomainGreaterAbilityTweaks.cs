using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class TravelDomainGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TravelDomainGreaterAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6; 
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "At 8th level, you can teleport up to 10 feet per level in the class that gave you access to this " +
                    "domain per day as a move action. This teleportation must be used in 5-foot increments and such " +
                    "movement does not provoke attacks of opportunity.\n" +
                    "The ability have a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
