using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class ProtectionDomainBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ProtectionDomainBaseAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var applyTarget = (ContextActionApplyBuff)c.Actions.Actions[0];
                    applyTarget.DurationValue.Rate = DurationRate.Rounds;
                    applyTarget.DurationValue.DiceType = DiceType.Zero;
                    applyTarget.DurationValue.DiceCountValue.Value = 0;
                    applyTarget.DurationValue.BonusValue.Value = 1;

                    var applySelf = (ContextActionApplyBuff)c.Actions.Actions[1];
                    applySelf.DurationValue.Rate = DurationRate.Rounds;
                    applySelf.DurationValue.DiceType = DiceType.Zero;
                    applySelf.DurationValue.DiceCountValue.Value = 0;
                    applySelf.DurationValue.BonusValue.Value = 1; 
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a swift action, you can touch an ally to grant them your resistance bonus for 1 round. " +
                    "When you do, you lose your Protection domain resistance bonus for 1 round."
                )
                .Configure();
        }
    }
}
