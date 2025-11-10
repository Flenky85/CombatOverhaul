using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexWindWardAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexWindWardAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var rootActions = c.Actions.Actions;
                    var applyMainBuff = (ContextActionApplyBuff)rootActions[0];
                    applyMainBuff.Permanent = false;
                    applyMainBuff.DurationValue.Rate = DurationRate.Rounds;
                    applyMainBuff.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    applyMainBuff.DurationValue.BonusValue.Value = 6;

                    var mainConditional = (Conditional)rootActions[1];
                    var conditionalDbdf = (Conditional)mainConditional.IfTrue.Actions[0];
                    
                    var applyAdd62 = (ContextActionApplyBuff)conditionalDbdf.IfTrue.Actions[0];
                    applyAdd62.DurationValue.Rate = DurationRate.Rounds;
                    applyAdd62.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    applyAdd62.DurationValue.BonusValue.Value = 6;

                    var applyD020Minutes = (ContextActionApplyBuff)conditionalDbdf.IfFalse.Actions[0];
                    applyD020Minutes.DurationValue.Rate = DurationRate.Rounds;
                    applyD020Minutes.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    applyD020Minutes.DurationValue.BonusValue.Value = 6;

                    var conditional6194 = (Conditional)mainConditional.IfTrue.Actions[1];
                    var apply48d4Minutes = (ContextActionApplyBuff)conditional6194.IfTrue.Actions[0];
                    apply48d4Minutes.DurationValue.Rate = DurationRate.Rounds;
                    apply48d4Minutes.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    apply48d4Minutes.DurationValue.BonusValue.Value = 6;

                    var applyD020Rounds = (ContextActionApplyBuff)mainConditional.IfFalse.Actions[0];
                    applyD020Rounds.DurationValue.Rate = DurationRate.Rounds;
                    applyD020Rounds.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    applyD020Rounds.DurationValue.BonusValue.Value = 6;

                    var conditional6cf9 = (Conditional)mainConditional.IfFalse.Actions[1];
                    var apply48d4Rounds = (ContextActionApplyBuff)conditional6cf9.IfTrue.Actions[0];
                    apply48d4Rounds.DurationValue.Rate = DurationRate.Rounds;
                    apply48d4Rounds.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    apply48d4Rounds.DurationValue.BonusValue.Value = 6;
                })
                .SetDescriptionValue(
                    "The shaman can touch a willing creature (including herself) and grants a ward of wind. " +
                    "This ward lasts for 6 rounds. When a warded creature is attacked with an arrow, ray, or other " +
                    "ranged attack that requires an attack roll, that attack suffers a 20% miss chance. At 16th level, " +
                    "the miss chance increases to 50%. Once affected, the creature cannot be the target of this hex " +
                    "again on new combat."
                )
                .Configure();
        }
    }
}
