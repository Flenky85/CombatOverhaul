using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class PlagueCaressAbilityCastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PlagueCaressAbilityCast)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3;
                })
                .SetDescriptionValue(
                    "As a standard action, the shaman can perform a melee touch attack that causes a creature " +
                    "to become sickened for 2d3 rounds. " +
                    "This is a disease effect.\n" +
                    "Additionally, this touch deals 1d6 points of acid damage per 2 shaman levels she possesses.\n" +
                    "Activating this ability expends 3 charges. The shaman has a number of charges equal to " +
                    "3 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
