using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class MagicFangGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MagicFangGreater)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var apply5 = (ContextActionApplyBuff)root.IfTrue.Actions[0];
                    apply5.UseDurationSeconds = false;
                    apply5.DurationValue.Rate = DurationRate.Rounds;
                    apply5.DurationValue.DiceType = DiceType.Zero;
                    apply5.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply5.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var cond4 = (Conditional)root.IfFalse.Actions[0];
                    var apply4 = (ContextActionApplyBuff)cond4.IfTrue.Actions[0];
                    apply4.UseDurationSeconds = false;
                    apply4.DurationValue.Rate = DurationRate.Rounds;
                    apply4.DurationValue.DiceType = DiceType.Zero;
                    apply4.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply4.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var cond3 = (Conditional)cond4.IfFalse.Actions[0];
                    var apply3 = (ContextActionApplyBuff)cond3.IfTrue.Actions[0];
                    apply3.UseDurationSeconds = false;
                    apply3.DurationValue.Rate = DurationRate.Rounds;
                    apply3.DurationValue.DiceType = DiceType.Zero;
                    apply3.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply3.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var cond2 = (Conditional)cond3.IfFalse.Actions[0];
                    var apply2 = (ContextActionApplyBuff)cond2.IfTrue.Actions[0];
                    apply2.UseDurationSeconds = false;
                    apply2.DurationValue.Rate = DurationRate.Rounds;
                    apply2.DurationValue.DiceType = DiceType.Zero;
                    apply2.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply2.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var cond1 = (Conditional)cond2.IfFalse.Actions[0];
                    var apply1 = (ContextActionApplyBuff)cond1.IfTrue.Actions[0];
                    apply1.UseDurationSeconds = false;
                    apply1.DurationValue.Rate = DurationRate.Rounds;
                    apply1.DurationValue.DiceType = DiceType.Zero;
                    apply1.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply1.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
