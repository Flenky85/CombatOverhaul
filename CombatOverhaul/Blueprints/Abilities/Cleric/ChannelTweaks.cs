using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class ChannelTweaks
    {
        public static void Register()
        {
            var abilities = new[]
            {
                AbilitiesGuids.ChannelEnergy,
                AbilitiesGuids.ChannelPositiveHarm,
                AbilitiesGuids.ChannelNegativeEnergy,
                AbilitiesGuids.ChannelNegativeHeal,
            };

            foreach (var id in abilities)
            {
                AbilityConfigurator.For(id)
                    .EditComponent<AbilityResourceLogic>(c =>
                    {
                        c.Amount = 6;
                    })
                    .Configure();
            }
        }
    }
}
