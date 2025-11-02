using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ShockingGraspCastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShockingGraspCast)
                .SetDescriptionValue(
                    "Your successful melee touch attack deals 1d8 points of electricity damage per caster level (maximum 4d8)."
                )
                .Configure();
        }
    }
}
