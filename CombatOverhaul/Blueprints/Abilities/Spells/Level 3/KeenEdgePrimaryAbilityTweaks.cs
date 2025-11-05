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
    internal static class KeenEdgePrimaryAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.KeenEdgePrimary)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var conditional = (Conditional)c.Actions.Actions[0];

                    var enhance = (EnhanceWeapon)conditional.IfFalse.Actions[0];
                    enhance.DurationValue.Rate = DurationRate.Rounds;
                    enhance.DurationValue.DiceType = DiceType.Zero;
                    enhance.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    enhance.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var apply = (ContextActionApplyBuff)conditional.IfFalse.Actions[1];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
