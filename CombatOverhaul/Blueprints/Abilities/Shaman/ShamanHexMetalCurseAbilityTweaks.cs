using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexMetalCurseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexMetalCurseAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman causes a creature within 30 feet to become slightly magnetic until the end of " +
                    "the shaman's next turn. Whenever the creature is attacked with a melee or ranged weapon " +
                    "constructed primarily of metal, it takes a –2 penalty to AC. At 8th and 16th levels, the " +
                    "penalty increases by –2 and the duration extends by 1 round. Once affected, the creature " +
                    "cannot be the target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
