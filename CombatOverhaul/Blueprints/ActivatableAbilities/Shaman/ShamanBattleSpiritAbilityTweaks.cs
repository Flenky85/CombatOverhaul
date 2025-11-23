using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Shaman
{
    [AutoRegister]
    internal static class ShamanBattleSpiritAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.ShamanBattleSpiritAbility)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Swift)
                .Configure();
        }
    }
}
