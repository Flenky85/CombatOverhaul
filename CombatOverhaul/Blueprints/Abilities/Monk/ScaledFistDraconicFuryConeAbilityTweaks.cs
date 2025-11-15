using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Collections.Generic;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class ScaledFistDraconicFuryConeAbilityTweaks
    {
        public static void Register()
        {
            var variants = new[]
            {
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistGoldBreathWeaponAbility,       "fire"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistGreenBreathWeaponAbility,      "acid"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistRedBreathWeaponAbility,        "fire"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistSilverBreathWeaponAbility,     "cold"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistWhiteBreathWeaponAbility,      "cold"),
            };
            foreach (var v in variants)
            {
                AbilityConfigurator.For(v.Key)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 8; })
                .SetDescriptionValue(
                    $"At 15th level, a scaled fist can spend 8 points from her ki pool to make a breath weapon attack as a standard action. " +
                    $"This breath weapon deals 1d6 points of {v.Value} damage per monk level in a 30-foot cone. Those caught in the area of the " +
                    $"breath can attempt a Reflex save (DC = 10 + 1/2 the scaled fist's monk level + her Charisma modifier) to halve the normal damage."
                )
                .Configure();
            }
        }
    }
}
