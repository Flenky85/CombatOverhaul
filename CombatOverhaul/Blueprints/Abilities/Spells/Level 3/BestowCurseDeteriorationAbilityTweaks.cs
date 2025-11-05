using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class BestowCurseDeteriorationAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BestowCurseDeterioration)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 8
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var dmgFull = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.NegativeEnergy
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank, ValueRank = AbilityRankType.Default },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        Half = false,
                        IsAoE = false
                    };

                    var dmgHalf = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.NegativeEnergy
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank, ValueRank = AbilityRankType.Default },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        Half = true,
                        IsAoE = false
                    };

                    var applyBuff = (ContextActionApplyBuff)cond.Failed.Actions[0];
                    cond.Failed.Actions = new GameAction[] { dmgFull, applyBuff };
                    cond.Succeed.Actions = new GameAction[] { dmgHalf };
                    c.Actions.Actions[0] = cond;
                })
                .Configure();
        }
    }
}
