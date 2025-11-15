using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Collections.Generic;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class ScaledFistDraconicFuryLineAbilityTweaks
    {
        public static void Register()
        {
            var variants = new[]
            {
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistBlackBreathWeaponAbility,       "acid"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistBlueBreathWeaponAbility,        "cold"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistBrassBreathWeaponAbility,       "fire"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistBronzeBreathWeaponAbility,      "electricity"),
                new KeyValuePair<string,string>(AbilitiesGuids.ScaledFistCopperBreathWeaponAbility,      "acid"),
            };
            foreach (var v in variants)
            {
                AbilityConfigurator.For(v.Key)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 8; })
                .SetDescriptionValue(
                    $"At 15th level, a scaled fist can spend 8 points from her ki pool to make a breath weapon attack as a standard action. " +
                    $"This breath weapon deals 1d6 points of {v.Value} damage per monk level in a 60-foot line. Those caught in the area of the " +
                    $"breath can attempt a Reflex save (DC = 10 + 1/2 the scaled fist's monk level + her Charisma modifier) to halve the normal damage."
                )
                .Configure();
            }
        }
    }
}
