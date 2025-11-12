using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class WitchDoctorChannelEnergyAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchDoctorChannelEnergy)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .Configure();
        }
    }
}
