using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class HexChannelerChannelNegativeHealAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HexChannelerChannelNegativeHeal)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6; 
                })
                .Configure();
        }
    }
}
