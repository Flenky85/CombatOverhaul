using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class AngelfireApostleCleansingFlamesAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AngelfireApostleCleansingFlamesAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6; 
                })
                .Configure();
        }
    }
}
