using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiPoisonCastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.KiPoisonCast)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 4; })
                .SetDescriptionValue(
                    "A monk with this ki power can spend 4 points from his ki pool as a standard action to infect the " +
                    "subject with a horrible poison by making a successful melee touch attack. This poison deals 1d3 " +
                    "Constitution damage per round for 6 rounds."
                )
                .Configure();
        }
    }
}
