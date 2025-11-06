using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class ProtectionFromEnergyCommunalAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ProtectionFromEnergyCommunal)
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "Protection from energy grants all allies within a 25-foot radius temporary immunity to the type of energy you specify when you " +
                    "cast it (acid, cold, electricity, fire, or sonic). When the spell absorbs 10 points per caster " +
                    "level of energy damage (to a maximum of 80 points at 8th level), it is discharged."
                )
                .Configure();
        }
    }
}
