using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class CommandUndeadAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CommandUndead)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var cond = (Conditional)saved.Failed.Actions[0];

                    var applyIfTrue = (ContextActionApplyBuff)cond.IfTrue.Actions[0];
                    var applyIfFalse = (ContextActionApplyBuff)cond.IfFalse.Actions[0];

                    static void Set2d3(ContextActionApplyBuff a)
                    {
                        a.DurationValue.DiceType = DiceType.D3;
                        a.DurationValue.DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 2
                        };
                        a.DurationValue.BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        };
                    }
                    Set2d3(applyIfTrue);
                    Set2d3(applyIfFalse);
                })
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(false)
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
