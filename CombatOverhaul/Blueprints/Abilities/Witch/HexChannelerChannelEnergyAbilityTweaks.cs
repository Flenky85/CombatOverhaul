using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class HexChannelerChannelEnergyAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HexChannelerChannelEnergy)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6; 
                })
                .Configure();
        }
    }
}
