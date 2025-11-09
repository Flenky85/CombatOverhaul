using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class HolySwordAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HolySword)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var e0 = (ContextActionEnchantWornItem)c.Actions.Actions[0];
                    e0.DurationValue.Rate = DurationRate.Rounds;
                    e0.DurationValue.DiceType = DiceType.Zero;
                    e0.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    e0.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                    e0.DurationValue.m_IsExtendable = false;

                    var e1 = (ContextActionEnchantWornItem)c.Actions.Actions[1];
                    e1.DurationValue.Rate = DurationRate.Rounds;
                    e1.DurationValue.DiceType = DiceType.Zero;
                    e1.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    e1.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                    e1.DurationValue.m_IsExtendable = false;

                    var e2 = (ContextActionEnchantWornItem)c.Actions.Actions[2];
                    e2.DurationValue.Rate = DurationRate.Rounds;
                    e2.DurationValue.DiceType = DiceType.Zero;
                    e2.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    e2.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                    e2.DurationValue.m_IsExtendable = false;
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
