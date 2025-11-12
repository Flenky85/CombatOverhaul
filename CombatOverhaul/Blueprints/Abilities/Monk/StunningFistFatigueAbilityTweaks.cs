using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class StunningFistFatigueAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.StunningFistFatigueAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .Configure();
        }
    }
}
