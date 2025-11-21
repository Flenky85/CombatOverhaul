using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class RuneDomainBaseAbilityAcidTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.RuneDomainBaseAbilityAcid)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can create a blast rune in a desired location. Any creature entering a " +
                    "5-foot area around the rune takes 1d6 acid damage plus 1 point for every two levels you possess in " +
                    "the class that gave you access to this domain. The rune lasts a number of rounds equal to your level " +
                    "in the class that gave you access to this domain."
                )
                .Configure();
        }
    }
}
