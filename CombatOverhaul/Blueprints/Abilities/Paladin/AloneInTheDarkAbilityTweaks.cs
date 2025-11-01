using CombatOverhaul.Guids;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class AloneInTheDarkAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AloneInTheDark)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 6; })
                .Configure();
        }
    }
}
