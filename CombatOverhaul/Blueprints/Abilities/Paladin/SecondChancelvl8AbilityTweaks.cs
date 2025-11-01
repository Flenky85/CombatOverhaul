using CombatOverhaul.Guids;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class SecondChancelvl8AbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SecondChancelvl8)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .Configure();
        }
    }
}
