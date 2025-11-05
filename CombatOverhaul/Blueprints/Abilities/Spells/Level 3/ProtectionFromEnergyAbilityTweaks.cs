using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ProtectionFromEnergyAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ProtectionFromEnergy)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "Protection from energy grants temporary immunity to the type of energy you specify when you " +
                    "cast it (acid, cold, electricity, fire, or sonic). When the spell absorbs 10 points per caster " +
                    "level of energy damage (to a maximum of 80 points at 8th level), it is discharged."
                )
                .Configure();
        }
    }
}
