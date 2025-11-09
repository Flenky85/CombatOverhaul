using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class ElementalSwarmAirAbilityTweak
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ElementalSwarmAir)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var spawn = (ContextActionSpawnMonster)c.Actions.Actions[0];
                    spawn.DurationValue.Rate = DurationRate.Rounds;
                    spawn.DurationValue.DiceType = DiceType.Zero;
                    spawn.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    spawn.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };
                    spawn.DurationValue.m_IsExtendable = false;

                    var waitBuff = (ContextActionApplyBuff)c.Actions.Actions[1];
                    waitBuff.DurationValue.Rate = DurationRate.Rounds;
                    waitBuff.DurationValue.DiceType = DiceType.Zero;
                    waitBuff.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    waitBuff.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    };
                    waitBuff.DurationValue.m_IsExtendable = false;
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
