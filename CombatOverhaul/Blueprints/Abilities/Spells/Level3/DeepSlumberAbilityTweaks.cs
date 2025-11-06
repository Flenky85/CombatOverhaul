using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class DeepSlumberAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DeepSlumber)
                .SetActionType(UnitCommand.CommandType.Standard)   
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];
                    var cond2 = (Conditional)root.IfFalse.Actions[0];

                    var cond3 = (Conditional)cond2.IfTrue.Actions[0];
                    var saveA = (ContextActionSavingThrow)cond3.IfTrue.Actions[0];
                    {
                        var csA = (ContextActionConditionalSaved)saveA.Actions.Actions[0];
                        var applyA = (ContextActionApplyBuff)csA.Failed.Actions[0];
                        applyA.UseDurationSeconds = false;
                        applyA.DurationValue.Rate = DurationRate.Rounds;
                        applyA.DurationValue.DiceType = DiceType.D3;
                        applyA.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                        applyA.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    }

                    var saveB = (ContextActionSavingThrow)cond2.IfFalse.Actions[0];
                    {
                        var csB = (ContextActionConditionalSaved)saveB.Actions.Actions[0];
                        var applyB = (ContextActionApplyBuff)csB.Failed.Actions[0];
                        applyB.UseDurationSeconds = false;
                        applyB.DurationValue.Rate = DurationRate.Rounds;
                        applyB.DurationValue.DiceType = DiceType.D3;
                        applyB.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                        applyB.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    }
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
