using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class CaveFangsStalagmitesAbilityAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CaveFangsStalagmitesAbility)
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
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)c.Actions.Actions[0];
                    spawn.DurationValue.Rate = DurationRate.Rounds;
                    spawn.DurationValue.DiceType = DiceType.Zero;
                    spawn.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    spawn.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
