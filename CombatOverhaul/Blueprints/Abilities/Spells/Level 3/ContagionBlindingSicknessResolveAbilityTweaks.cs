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
    internal static class ContagionBlindingSicknessAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ContagionBlindingSicknessResolve)
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
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];

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
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.Default
                            },
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
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.Default
                            },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        Half = true,   
                        IsAoE = false
                    };

                    var succeedPrev = saved.Succeed.Actions[0];
                    var failedPrev = saved.Failed.Actions[0];

                    saved.Succeed.Actions = new GameAction[] { dmgHalf, succeedPrev };
                    saved.Failed.Actions = new GameAction[] { dmgFull, failedPrev };

                    c.Actions.Actions[0] = saved;
                })
                .Configure();
        }
    }
}
