using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class DragonRoarAbility
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DragonRoarAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 6; })
                .Configure();
        }
    }
}
