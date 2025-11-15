using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexLifeGiverAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexLifeGiverAbility)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "Once per day the witch can, as a standard action, touch a dead creature and bring it back to life. " +
                    "This functions as resurrection, but it does not require a material component."
                )
                .Configure();
        }
    }
}
