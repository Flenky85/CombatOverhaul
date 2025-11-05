using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class HypnotismAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Hypnotism)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];
                    var condB = (Conditional)root.IfFalse.Actions[0];
                    var condB_IfTrue = (Conditional)condB.IfTrue.Actions[0];
                    var save1 = (ContextActionSavingThrow)condB_IfTrue.IfTrue.Actions[0];
                    var cs1 = (ContextActionConditionalSaved)save1.Actions.Actions[0];
                    var apply1 = (ContextActionApplyBuff)cs1.Failed.Actions[0];

                    apply1.UseDurationSeconds = false;
                    apply1.DurationValue.Rate = DurationRate.Rounds;
                    apply1.DurationValue.DiceType = DiceType.D3;
                    apply1.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    apply1.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var save2 = (ContextActionSavingThrow)condB.IfFalse.Actions[0];
                    var cs2 = (ContextActionConditionalSaved)save2.Actions.Actions[0];
                    var apply2 = (ContextActionApplyBuff)cs2.Failed.Actions[0];

                    apply2.UseDurationSeconds = false;
                    apply2.DurationValue.Rate = DurationRate.Rounds;
                    apply2.DurationValue.DiceType = DiceType.D3;
                    apply2.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    apply2.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
