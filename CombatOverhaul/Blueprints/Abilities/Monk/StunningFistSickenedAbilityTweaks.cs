using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class StunningFistSickenedAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.StunningFistSickenedAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .Configure();
        }
    }
}
