using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexDeathCurseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexDeathCurseAbility)
                .SetDescriptionValue(
                    "This hex has a range of 30 feet. The hexed creature receives a Will save to negate the effect. " +
                    "If this save is failed, the creature becomes fatigued the first round of the hex. On the second " +
                    "round of the hex, the creature becomes exhausted. On the third round, the creature dies unless " +
                    "it succeeds at a Fort save. Creatures that fail the first save but succeed at the second remain " +
                    "exhausted and take 4d6 points of damage + 1 point of damage per level of the witch. Slaying the " +
                    "witch that hexed the creature ends the effect, but any fatigue or exhaustion remains. Whether or " +
                    "not the saves are successful, a creature cannot be the target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
