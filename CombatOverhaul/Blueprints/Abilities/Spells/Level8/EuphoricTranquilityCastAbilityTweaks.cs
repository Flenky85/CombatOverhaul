using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class EuphoricTranquilityCastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.EuphoricTranquilityCast)
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
