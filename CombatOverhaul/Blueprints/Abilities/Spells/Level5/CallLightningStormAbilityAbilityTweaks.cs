using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class CallLightningStormAbilityAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CallLightningStormAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityExecuteActionOnCast>(c =>
                {
                    var reduce = (ContextActionReduceBuffDuration)c.Actions.Actions[0];
                    reduce.DurationValue.Rate = DurationRate.Rounds;
                    reduce.DurationValue.DiceType = DiceType.Zero;
                    reduce.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    reduce.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };
                })
                .Configure();
        }
    }
}
