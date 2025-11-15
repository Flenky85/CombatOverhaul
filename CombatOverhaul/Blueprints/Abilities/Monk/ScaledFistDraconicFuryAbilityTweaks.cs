using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Collections.Generic;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class ScaledFistDraconicFuryAbilityTweaks
    {
        public static void Register()
        {
            var variants = new[]
            {
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistDraconicFuryAcidAbility,        "acid"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistDraconicFuryColdAbility,        "cold"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistDraconicFuryElectricityAbility, "electricity"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistDraconicFuryFireAbility,        "fire"),
            };
            foreach (var v in variants)
            {
                AbilityConfigurator.For(v.Key)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    $"The scaled fist can expend 3 points from her ki pool as a swift action to imbue her natural attacks with {v.Value}, " +
                    $"causing them to deal an extra 1d6 points of {v.Value} damage for a number of rounds equal to 1/2 her monk level."
                )
                .Configure();
            }
        }
    }
}
