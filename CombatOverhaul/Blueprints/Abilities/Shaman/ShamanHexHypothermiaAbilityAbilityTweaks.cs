using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexHypothermiaAbilityAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexHypothermiaAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman afflicts a creature within 30 feet with hypothermia. The target must attempt a " +
                    "Fortitude saving throw. On a failed save, the target is fatigued for 2 rounds. At 8th and 16th " +
                    "levels, the duration of this hex is extended by 1 round. Whether or not the save is successful, " +
                    "the creature cannot be the target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
