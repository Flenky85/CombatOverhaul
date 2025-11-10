using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexBattleWardAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexBattleWardAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman touches a willing creature (including herself) and grants a battle ward. " +
                    "The next time a foe makes an attack roll against the target, the ward activates and " +
                    "grants a +3 deflection bonus to the warded creature's AC. Each subsequent time she's " +
                    "attacked, the deflection bonus reduces by 1 (to +2 for the second time she's attacked " +
                    "and +1 for the third). The ward fades when the bonus is reduced to +0 or after combat, " +
                    "whichever comes first. At 8th level, the ward's starting bonus increases to +4. At 16th " +
                    "level, it increases to +5. A creature affected by this hex cannot be affected by it " +
                    "again on new combat."
                )
                .Configure();
        }
    }
}
