using CombatOverhaul.Guids;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class SecondChanceAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SecondChance)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .Configure();
        }
    }
}
