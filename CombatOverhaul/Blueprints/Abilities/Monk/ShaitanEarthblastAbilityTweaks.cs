using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class ShaitanEarthblastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShaitanEarthblastAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 4; })
                .SetDescriptionValue(
                    "Benefit: While using the Shaitan Style, as a standard action, you can spend two " +
                    "Elemental Fist charges to unleash a column of acid that has a 10-foot radius and " +
                    "erupts from a point of origin within 30 feet of you. Creatures caught in the column " +
                    "take your unarmed strike damage plus the acid damage from your Elemental Fist and are " +
                    "staggered for 1 round. A successful Reflex save (DC = 10 + half your character level + your Wisdom modifier) " +
                    "reduces the damage by half and prevents the target from being staggered."
                )
                .Configure();
        }
    }
}
