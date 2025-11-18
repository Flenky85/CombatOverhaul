using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class LeyLineGuardianConduitSurgeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.LeyLineGuardianConduitSurgeAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3; 
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "At 1st level, a ley line guardian is adept at channeling energy from ley lines to enhance her own spells. " +
                    "As a swift action, she can increase her effective caster level for the next spell she casts in that round by 1d4–1 levels. " +
                    "After performing a conduit surge, the ley line guardian must succeed at a Fortitude save (DC = 10 + level of spell cast + " +
                    "number of additional caster levels granted) or become staggered for a number of rounds equal to the level of the spell cast. " +
                    "At 8th level, the caster level increase becomes 1d4.\n " +
                    "The class begins with 3 charges; at 10th level and " +
                    "20th level she gains 3 additional charges each time (for a total of 6 and 9 charges, " +
                    "respectively). At the start of each of her turns, she regains 1 expended charge, but only " +
                    "while the buff is not active."
                )
                .Configure();
        }
    }
}
