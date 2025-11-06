using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class DragonsBreathAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DragonsBreath)
                .SetDescriptionValue(
                    "You breathe out a blast of energy. Creatures in the affected area take 1d6 points of energy " +
                    "damage per caster level (maximum 10d6) if the attack is a cone, or 1d8 per caster level " +
                    "(maximum 10d8) if the attack is a line. A successful Reflex save halves the damage. The " +
                    "spell’s effect and energy type depend on the type of dragon."
                )
                .Configure();
        }
    }
}
