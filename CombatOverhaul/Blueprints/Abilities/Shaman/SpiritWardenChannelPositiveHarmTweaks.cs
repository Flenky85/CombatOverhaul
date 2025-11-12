using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class SpiritWardenChannelPositiveHarmTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SpiritWardenChannelPositiveHarm)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .Configure();
        }
    }
}
