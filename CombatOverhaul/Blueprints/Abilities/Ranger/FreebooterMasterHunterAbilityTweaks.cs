using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Ranger
{
    [AutoRegister]
    internal static class FreebooterMasterHunterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FreebooterMasterHunterAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 4;
                })
                .SetDescriptionValue(
                    "At 20th level, freebooter becomes a master hunter. She can, as a standard action, make a single attack against a target of " +
                    "freebooter's bane at her full attack bonus. If the attack hits, the target takes damage normally and must make a Fortitude " +
                    "save or die. The DC of this save is equal to 10 + 1/2 the freebooter's level + the freebooter's Wisdom modifier. " +
                    "The freebooter cannot use this ability on the same creature more than once per combat.\n" +
                    "The ability have a cooldown of 4 rounds."
                )
                .Configure();
        }
    }
}
