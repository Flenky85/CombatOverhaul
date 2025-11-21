using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class MadnessDomainBaseAbilitySavingThrowsTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MadnessDomainBaseAbilitySavingThrows)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "You can give a creature a vision of madness as a melee touch attack. The target receives a " +
                    "bonus to saving throws equal to 1/2 your level in the class that gave you access to this " +
                    "domain (minimum +1) and a penalty to attack rolls and skill checks equal to 1/2 your level " +
                    "in the class that gave you access to this domain (minimum –1). This effect fades after 3 rounds."
                )
                .Configure();
        }
    }
}
