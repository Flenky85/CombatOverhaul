using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class DivinationSchoolBaseAbilityCastTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DivinationSchoolBaseAbilityCast)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "When you activate this school power, you can touch any creature as a standard action to give it an " +
                    "insight bonus on all of its attack rolls, skill checks, ability checks, and saving throws equal to 1/2 " +
                    "your wizard level (minimum +1) for 1 round."
                )
                .Configure();
        }
    }
}
