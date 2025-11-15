using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level1
{
    [AutoRegister]
    internal static class IllOmenAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.IllOmen)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;   
                    c.m_Progression = ContextRankProgression.StartPlusDivStep;
                    c.m_StartLevel = 0;
                    c.m_StepLevel = 2;   
                    c.m_UseMax = true;
                    c.m_Max = 3;
                })

                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var applyMain = (ContextActionApplyBuff)c.Actions.Actions[1];
                    applyMain.DurationValue.Rate = DurationRate.Rounds;
                    applyMain.DurationValue.DiceType = DiceType.Zero;
                    applyMain.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyMain.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };

                    var cond = (Conditional)c.Actions.Actions[2];

                    var firstApply = (ContextActionApplyBuff)cond.IfTrue.Actions[0];
                    firstApply.DurationValue.Rate = DurationRate.Rounds;
                    firstApply.DurationValue.DiceType = DiceType.Zero;
                    firstApply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    firstApply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };

                    var c_ge1 = (Conditional)cond.IfTrue.Actions[1];
                    var cmp1 = (ContextConditionCompare)c_ge1.ConditionsChecker.Conditions[0];
                    cmp1.TargetValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };
                    var apply2 = (ContextActionApplyBuff)c_ge1.IfTrue.Actions[0];
                    apply2.DurationValue.Rate = DurationRate.Rounds;
                    apply2.DurationValue.DiceType = DiceType.Zero;
                    apply2.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply2.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };

                    var c_ge2 = (Conditional)cond.IfTrue.Actions[2];
                    var cmp2 = (ContextConditionCompare)c_ge2.ConditionsChecker.Conditions[0];
                    cmp2.TargetValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    var apply3 = (ContextActionApplyBuff)c_ge2.IfTrue.Actions[0];
                    apply3.DurationValue.Rate = DurationRate.Rounds;
                    apply3.DurationValue.DiceType = DiceType.Zero;
                    apply3.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply3.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };

                    var c_offA = (Conditional)cond.IfTrue.Actions[3];
                    var cmpOffA = (ContextConditionCompare)c_offA.ConditionsChecker.Conditions[0];
                    cmpOffA.TargetValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1000 };

                    var c_offB = (Conditional)cond.IfTrue.Actions[4];
                    var cmpOffB = (ContextConditionCompare)c_offB.ConditionsChecker.Conditions[0];
                    cmpOffB.TargetValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1000 };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "On the next d20 roll the target makes, it must roll twice and take the less favorable result. " +
                    "For every two caster levels you have, the target must roll twice on an additional d20 roll " +
                    "(to a maximum of 3 rolls at level 4)."
                )
                .Configure();
        }
    }
}
