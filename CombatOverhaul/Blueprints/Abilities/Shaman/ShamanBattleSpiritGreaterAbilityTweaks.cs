using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanBattleSpiritGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanBattleSpiritGreaterAbility)
                .SetDescriptionValue(
                    "As a swift action, the shaman imbues a single weapon she's wielding with the bane " +
                    "weapon special ability, choosing the type of creature affected each time she does. " +
                    "The effect lasts for 3 rounds. If the weapon already has the bane weapon special " +
                    "ability of the type chosen, the additional damage dealt by bane increases to 4d6.\n" +
                    "Activating this form expends 3 charges. The shaman has a number of charges equal to " +
                    "3 plus her Charisma modifier. At the start of each of her turns, she regains 1 " +
                    "expended charge, but only while this form is not active."
                )
                .Configure();
        }
    }
}
