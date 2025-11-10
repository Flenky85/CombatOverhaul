using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexHamperingHexAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexHamperingHexAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman causes a creature within 30 feet to take a –2 penalty to AC and CMD for a " +
                    "number of rounds equal to the shaman's level. A successful Will saving throw reduces " +
                    "this to just 1 round. At 8th level, the penalty becomes –4. Whether or not the save is " +
                    "successful, a creature affected by a hampering hex cannot be the target of this hex " +
                    "again on new combat."
                )
                .Configure();
        }
    }
}
