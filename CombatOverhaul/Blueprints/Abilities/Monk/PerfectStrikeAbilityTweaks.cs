using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class PerfectStrikeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PerfectStrikeAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "You must declare that you are using this feat before you make your attack roll " +
                    "(thus a failed attack roll ruins the attempt). You can roll your attack roll twice " +
                    "and take the higher result. You may attempt a perfect attack once per day for every " +
                    "four levels you have attained (but see Special).\n" +
                    "This ability has a cooldown of 3 rounds."
                )
                .Configure();
        }
    }
}
