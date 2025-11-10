using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexFlameCurseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexFlameCurseAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman causes a creature within 30 feet to become vulnerable to fire until the end of the shaman's " +
                    "next turn. If the creature is already vulnerable to fire, this hex has no effect. Fire immunity and " +
                    "resistances apply as normal, and any saving throw allowed by the effect that caused the damage reduces " +
                    "it as normal. At 8th and 16th levels, the duration of this hex is extended by 1 round. A creature affected " +
                    "by this hex cannot be affected by it again on new combat."
                )
                .Configure();
        }
    }
}
