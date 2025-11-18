using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class ArcaneBombTweaks
    {
        public static void Register()
        {
            var buffs = new[]
            {
                AbilitiesGuids.ArcaneBombsAcidAbility,
                AbilitiesGuids.ArcaneBombsColdAbility,
                AbilitiesGuids.ArcaneBombsElectricityAbility,
                AbilitiesGuids.ArcaneBombsFireAbility,

            };

            foreach (var id in buffs)
            {
                AbilityConfigurator.For(id)
                    .EditComponent<AbilityResourceLogic>(c =>
                    {
                        c.Amount = 3;
                    })
                    .Configure();
            }
        }
    }
}
