using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class CommandGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CommandGreater)
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
