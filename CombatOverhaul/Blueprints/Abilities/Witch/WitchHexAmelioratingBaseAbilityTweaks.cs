using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexAmelioratingBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexAmelioratingBaseAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDescriptionValue(
                    "The witch can touch a creature to protect it from negative conditions and suppress their effects. " +
                    "The witch chooses one of the following conditions each time she uses this hex: dazzled, fatigued, " +
                    "shaken, or sickened. If the target is afflicted with the chosen condition, that condition is suppressed " +
                    "for two rounds and an aditional round per 5th witch's level. Additionally, the witch grants her target a +4 " +
                    "circumstance bonus on saving throws against effects that cause the chosen conditions for one combat. " +
                    "A creature can benefit from the hex twice per combat, once for each of two different conditions."
                )
                .Configure();
        }
    }
}
