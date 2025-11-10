using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexAmelioratingBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexAmelioratingBaseAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The shaman can touch a creature to protect it from negative conditions and suppress their effects. " +
                    "The shaman chooses one of the following conditions each time she uses this hex: dazzled, fatigued, " +
                    "shaken, or sickened. If the target is afflicted with the chosen condition, that condition is suppressed " +
                    "for two rounds and an aditional round per 5th shaman's level. Additionally, the shaman grants her target a +4 " +
                    "circumstance bonus on saving throws against effects that cause the chosen conditions for one combat. " +
                    "A creature can benefit from the hex twice per combat, once for each of two different conditions."
                )
                .Configure();
        }
    }
}
