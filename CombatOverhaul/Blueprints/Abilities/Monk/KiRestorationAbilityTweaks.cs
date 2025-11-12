using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiRestorationAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.KiRestoration)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 4; })
                .SetDescriptionValue(
                    "A monk with this ki power can spend 4 points from his ki pool as a standard action to " +
                    "dispel temporary negative levels or one permanent negative level.\n" +
                    "Restoration cures all temporary ability damage, and it restores all points permanently " +
                    "drained from a single ability score.It also eliminates any fatigue or exhaustion suffered by the target."
                )
                .Configure();
        }
    }
}
