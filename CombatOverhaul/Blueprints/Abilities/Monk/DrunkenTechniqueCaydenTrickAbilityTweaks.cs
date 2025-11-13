using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class DrunkenTechniqueCaydenTrickAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DrunkenTechniqueCaydenTrickAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 4; })
                .SetDescriptionValue(
                    "At 6th level, as a standard action, a drunken master can spend 4 drunken ki points to unleash a 15-foot cone of beverage, " +
                    "blinding and hindering creatures caught within. They must succeed at a Reflex save (DC 10 + 1/2 the monk's level + 1/2 the " +
                    "monk's Wisdom modifier) or suffer a –2 penalty to AC and become blinded, flat-footed, and sickened for 1d4+1 rounds. " +
                    "The ground in the area also becomes slick as if affected by a grease spell."
                )
                .Configure();
        }
    }
}
