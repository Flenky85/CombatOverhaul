using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Rogue
{
    [AutoRegister]
    internal static class MasterStrikeToggleAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.MasterStrikeToggleAbility)
                .SetDescriptionValue(
                    "Upon reaching 20th level, a rogue becomes incredibly deadly when dealing sneak attack damage. " +
                    "Each time the rogue deals sneak attack damage, she can slay the target. The target receives a " +
                    "Fortitude save to negate the death effect. The DC of this save is equal to 10 + 1/2 the rogue's " +
                    "level + the rogue's Dexterity modifier. Once a creature has been the target of a master strike, " +
                    "regardless of whether or not the save is made, that creature is immune to that rogue's master strike " +
                    "for the rest of the encounter."
                )
                .Configure();
        }
    }
}
