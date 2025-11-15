using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexAnimalServantAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexAnimalServantAbility)
                .SetDescriptionValue(
                    "The witch can use this hex to turn a humanoid enemy into an animal and rob it of its free will.\n" +
                    "The transformation works as beast shape II and is negated by a successful Will save.The transformed " +
                    "creature retains its Intelligence score and known languages, if any, but the witch controls its mind. " +
                    "This effect functions as dominate monster, except the creature does not receive further saving throws to " +
                    "resist the hex.Whether or not the save is successful, a creature cannot be the target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
