using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Ranger
{
    [AutoRegister]
    internal static class MasterSpyAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MasterSpyAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 5;
                })
                .SetDescriptionValue(
                    "An espionage expert of 20th level becomes an espionage master. She can, as a standard action, " +
                    "make a single attack against a favored enemy at her full attack bonus. If the attack hits, the " +
                    "target takes damage normally and must make a Fortitude save or die. The DC of this save is equal " +
                    "to 10 + 1/2 the espionage expert's level + the espionage expert's Charisma modifier. An espionage " +
                    "expert can't use this ability against the same creature more than once in a 24-hour period.\n" +
                    "The ability have a cooldown of 5 rounds."
                )
                .Configure();
        }
    }
}
