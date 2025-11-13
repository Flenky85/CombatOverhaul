using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiAbudantStepAbilityTweaks
    {
        public static void Register()
        {
            var abilites = new[]
            {               
                AbilitiesGuids.KiAbudantStep,
                AbilitiesGuids.DrunkenKiAbudantStep,
                AbilitiesGuids.ScaledFistAbudantStep,
            };
            foreach (var id in abilites)
            {
                AbilityConfigurator.For(id)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 4; })
                .SetDescriptionValue(
                    "At 8th level or higher, a monk can slip magically between spaces, as if using the spell dimension door. " +
                    "Using this ability is a move action that consumes 4 points from his ki pool. He cannot take other creatures " +
                    "with him when he uses this ability."
                )
                .Configure();
            }
        }
    }
}
