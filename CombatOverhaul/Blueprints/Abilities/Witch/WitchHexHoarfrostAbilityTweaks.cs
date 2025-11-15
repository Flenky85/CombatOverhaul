using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexHoarfrostAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexHoarfrostAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The target is rimed with a shell of frost needles that slowly work their way into its flesh " +
                    "(Fortitude negates). The target turns pale and blue, and takes 1 point of Constitution damage per " +
                    "round until it dies, saves (once per round), or is cured. Break enchantment, dispel magic, remove " +
                    "curse, and similar spells end the effect. If the target saves, it is immune to this hex on new combat. " +
                    "This is a cold effect."
                )
                .Configure();
        }
    }
}
