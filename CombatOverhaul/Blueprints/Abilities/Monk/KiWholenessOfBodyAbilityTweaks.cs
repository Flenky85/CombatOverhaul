using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiWholenessOfBodyAbilityTweaks
    {
        public static void Register()
        {
            var abilites = new[]
            {
                AbilitiesGuids.KiWholenessOfBody,
                AbilitiesGuids.DrunkenKiWholenessOfBody,
                AbilitiesGuids.ScaledFistWholenessOfBody,
            };
            foreach (var id in abilites)
            {
                AbilityConfigurator.For(id)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 4; })
                .SetDescriptionValue(
                    "By spending 4 points from his ki pool as a swift action, the monk can heal a " +
                    "number of hit points of damage equal to his monk level."
                )
                .Configure();
            }
        }
    }
}
