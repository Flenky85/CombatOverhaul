using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class PhantasmalPutrefactionAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PhantasmalPutrefaction)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var condRoot = (Conditional)c.Actions.Actions[0];
                    var cond2 = (Conditional)condRoot.IfFalse.Actions[0];
                    var cond3 = (Conditional)cond2.IfTrue.Actions[0];
                    var saveA = (ContextActionSavingThrow)cond3.IfTrue.Actions[0];
                    var csA = (ContextActionConditionalSaved)saveA.Actions.Actions[0];
                    var appA = (ContextActionApplyBuff)csA.Failed.Actions[0];

                    appA.UseDurationSeconds = false;
                    appA.DurationValue.Rate = DurationRate.Rounds;
                    appA.DurationValue.DiceType = DiceType.D3;
                    appA.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    appA.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var saveB = (ContextActionSavingThrow)cond2.IfFalse.Actions[0];
                    var csB = (ContextActionConditionalSaved)saveB.Actions.Actions[0];
                    var appB = (ContextActionApplyBuff)csB.Failed.Actions[0];

                    appB.UseDurationSeconds = false;
                    appB.DurationValue.Rate = DurationRate.Rounds;
                    appB.DurationValue.DiceType = DiceType.D3;
                    appB.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    appB.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
