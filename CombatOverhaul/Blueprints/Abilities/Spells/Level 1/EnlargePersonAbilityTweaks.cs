using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class EnlargePersonAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.EnlargePerson)
                .SetActionType(UnitCommand.CommandType.Standard)   
                .SetIsFullRoundAction(false)                       
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];
                    var inner = (Conditional)root.IfFalse.Actions[0];
                    var nested = (Conditional)inner.IfTrue.Actions[0];
                    var applyA = (ContextActionApplyBuff)nested.IfTrue.Actions[0];
                    applyA.DurationValue.Rate = DurationRate.Rounds;
                    applyA.DurationValue.DiceType = DiceType.Zero;
                    applyA.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyA.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var applyB = (ContextActionApplyBuff)nested.IfFalse.Actions[0];
                    applyB.DurationValue.Rate = DurationRate.Rounds;
                    applyB.DurationValue.DiceType = DiceType.Zero;
                    applyB.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyB.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var applyC = (ContextActionApplyBuff)inner.IfFalse.Actions[0];
                    applyC.DurationValue.Rate = DurationRate.Rounds;
                    applyC.DurationValue.DiceType = DiceType.Zero;
                    applyC.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyC.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                })
                .SetDurationValue("6 rounds")
                .Configure();
        }
    }
}
