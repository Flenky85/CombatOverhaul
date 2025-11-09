using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level9
{
    [AutoRegister]
    internal static class ElementalSwarmWaterThirdWaveBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.ElementalSwarmWaterThirdWaveBuff)
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
                        Value = 4
                    };
                    spawn.DurationValue.m_IsExtendable = false;
                })
                .Configure();
        }
    }
}
