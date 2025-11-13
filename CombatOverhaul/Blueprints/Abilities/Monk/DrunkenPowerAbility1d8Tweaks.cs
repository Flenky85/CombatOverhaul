using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class DrunkenPowerAbility1d8Tweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DrunkenPowerAbility1d8)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "At 5th level, as a swift action, a drunken master can spend 3 drunken ki point to make his melee " +
                    "attacks deal additional damage for a number of rounds equal to his Wisdom modifier: 1d8 damage at " +
                    "5th level, 2d8 damage at 8th level, 3d8 damage at 11th level, 4d8 damage at 14th level, " +
                    "and 5d8 damage at 18th level."
                )
                .Configure();
        }
    }
}
