using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class ProtectiveWardAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ProtectiveWardAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can create a 10-foot-radius field of protective magic centered on you that lasts " +
                    "for a number of rounds equal to your Intelligence modifier. All allies in this area (including you) receive a " +
                    "+1 deflection bonus to their Armor Class. This bonus increases by +1 for every five wizard levels you possess.\n" +
                    "The aura have a cooldown of 3 rounds."
                )
                .Configure();
        }
    }
}
