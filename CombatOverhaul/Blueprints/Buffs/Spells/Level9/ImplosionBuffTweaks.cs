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
    internal static class ImplosionBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.ImplosionBuff)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,          
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 20
                })
                .EditComponent<AddBuffActions>(c =>
                {
                    var saving = (ContextActionSavingThrow)c.Activated.Actions[1];
                    var cond = (ContextActionConditionalSaved)saving.Actions.Actions[0];
                    var dmg = (ContextActionDealDamage)cond.Failed.Actions[0];

                    dmg.Value.DiceType = DiceType.D6;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .Configure();
        }
    }
}
