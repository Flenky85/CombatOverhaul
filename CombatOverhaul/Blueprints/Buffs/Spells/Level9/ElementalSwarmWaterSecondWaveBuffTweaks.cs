using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level9
{
    [AutoRegister]
    internal static class ElementalSwarmWaterSecondWaveBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.ElementalSwarmWaterSecondWaveBuff)
                .EditComponent<AddFactContextActions>(c =>
                {
                    var spawn = (ContextActionSpawnMonster)c.Deactivated.Actions[0];
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
                        Value = 5
                    };
                    spawn.DurationValue.m_IsExtendable = false;

                    var nextWaitBuff = (ContextActionApplyBuff)c.Deactivated.Actions[1];
                    nextWaitBuff.DurationValue.Rate = DurationRate.Rounds;
                    nextWaitBuff.DurationValue.DiceType = DiceType.Zero;
                    nextWaitBuff.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    nextWaitBuff.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    };
                    nextWaitBuff.DurationValue.m_IsExtendable = false;
                })
                .Configure();
        }
    }
}
