using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class EnchantmentSchoolBaseAbilityCastTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.EnchantmentSchoolBaseAbilityCast)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "You can cause a living creature to become dazed for 1 round as a melee touch attack. " +
                    "Creatures with more Hit Dice than your wizard level are unaffected."
                )
                .Configure();
        }
    }
}
