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
    internal static class MagicalVestmentShieldAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MagicalVestmentShield)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)cond.IfTrue.Actions[0];
                    apply.Permanent = false;
                    apply.UseDurationSeconds = false;
                    apply.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        },
                        BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 6
                        },
                        m_IsExtendable = true
                    };
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
