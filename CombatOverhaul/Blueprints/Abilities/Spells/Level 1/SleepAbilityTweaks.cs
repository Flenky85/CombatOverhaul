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


namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class SleepAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Sleep)
               .SetActionType(UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];
                    var cond2 = (Conditional)root.IfFalse.Actions[0];

                    var cond3 = (Conditional)cond2.IfTrue.Actions[0];
                    var saveA = (ContextActionSavingThrow)cond3.IfTrue.Actions[0];
                    SetFailedApplyBuffDurationTo2d3Rounds(saveA);

                    var saveB = (ContextActionSavingThrow)cond2.IfFalse.Actions[0];
                    SetFailedApplyBuffDurationTo2d3Rounds(saveB);

                    static void SetFailedApplyBuffDurationTo2d3Rounds(ContextActionSavingThrow save)
                    {
                        var cs = save.Actions.Actions.OfType<ContextActionConditionalSaved>().FirstOrDefault();
                        if (cs == null) return;

                        var apply = cs.Failed.Actions.OfType<ContextActionApplyBuff>().FirstOrDefault();
                        if (apply == null) return;

                        apply.UseDurationSeconds = false;
                        apply.DurationValue.Rate = DurationRate.Rounds;
                        apply.DurationValue.DiceType = DiceType.D3;
                        apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                        apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    }
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
