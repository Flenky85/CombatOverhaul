using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class HoldPersonMassAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HoldPersonMass)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var baseCond = (Conditional)c.Actions.Actions[0];
                    var baseSave = (ContextActionSavingThrow)baseCond.IfTrue.Actions[0];
                    var baseCondSaved = (ContextActionConditionalSaved)baseSave.Actions.Actions[0];
                    var baseApply = (ContextActionApplyBuff)baseCondSaved.Failed.Actions[0];
                    baseApply.UseDurationSeconds = false;
                    baseApply.Permanent = false;
                    baseApply.DurationValue.Rate = DurationRate.Rounds;
                    baseApply.DurationValue.DiceType = DiceType.D3;
                    baseApply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    baseApply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var serpTop = (Conditional)c.Actions.Actions[1];
                    var serpInner = (Conditional)serpTop.IfTrue.Actions[0];
                    var serpSave = (ContextActionSavingThrow)serpInner.IfTrue.Actions[0];
                    var serpCondSaved = (ContextActionConditionalSaved)serpSave.Actions.Actions[0];
                    var serpApply = (ContextActionApplyBuff)serpCondSaved.Failed.Actions[0];
                    serpApply.UseDurationSeconds = false;
                    serpApply.Permanent = false;
                    serpApply.DurationValue.Rate = DurationRate.Rounds;
                    serpApply.DurationValue.DiceType = DiceType.D3;
                    serpApply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    serpApply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var undTop = (Conditional)c.Actions.Actions[2];
                    var undInner = (Conditional)undTop.IfTrue.Actions[0];
                    var undSave = (ContextActionSavingThrow)undInner.IfTrue.Actions[0];
                    var undCondSaved = (ContextActionConditionalSaved)undSave.Actions.Actions[0];
                    var undApply = (ContextActionApplyBuff)undCondSaved.Failed.Actions[0];
                    undApply.UseDurationSeconds = false;
                    undApply.Permanent = false;
                    undApply.DurationValue.Rate = DurationRate.Rounds;
                    undApply.DurationValue.DiceType = DiceType.D3;
                    undApply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    undApply.DurationValue.BonusValue = new ContextValue
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
