using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class SunbeamAbilityTweaks
    {
        private const string UndeadFactId = "734a29b693e9ec346ba2951b27987e33";
        private const string BlindnessBuffId = "187f88d96a0ef464280706b63635f2af";
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Sunbeam)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    if (cfg.m_Type == AbilityRankType.DamageBonus)
                    {
                        cfg.m_UseMax = true;
                        cfg.m_Max = 16;
                        cfg.m_AffectedByIntensifiedMetamagic = false;
                    }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = SavingThrowType.Reflex;

                    var isUndead = new ContextConditionHasFact
                    {
                        m_Fact = BlueprintTool.GetRef<BlueprintUnitFactReference>(UndeadFactId),
                        Not = false
                    };

                    var dmgUndead = new ContextActionDealDamage
                    {
                        m_Type = ContextActionDealDamage.Type.Damage,
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Divine
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D8,
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.DamageBonus
                            },
                            BonusValue = new ContextValue
                            {
                                ValueType = ContextValueType.Simple,
                                Value = 0
                            }
                        },
                        HalfIfSaved = true,
                        IsAoE = true
                    };

                    var dmgLiving = new ContextActionDealDamage
                    {
                        m_Type = ContextActionDealDamage.Type.Damage,
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Divine
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D3,
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.DamageBonus
                            },
                            BonusValue = new ContextValue
                            {
                                ValueType = ContextValueType.Simple,
                                Value = 0
                            }
                        },
                        HalfIfSaved = true,
                        IsAoE = true
                    };

                    var dmgSelector = new Conditional
                    {
                        ConditionsChecker = new ConditionsChecker
                        {
                            Operation = Operation.And,
                            Conditions = new Condition[] { isUndead }
                        },
                        IfTrue = new ActionList { Actions = new GameAction[] { dmgUndead } },
                        IfFalse = new ActionList { Actions = new GameAction[] { dmgLiving } }
                    };

                    var blindOnFail = new ContextActionConditionalSaved
                    {
                        Succeed = new ActionList { Actions = new GameAction[0] },
                        Failed = new ActionList
                        {
                            Actions = new GameAction[]
                            {
                                new ContextActionApplyBuff
                                {
                                    m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(BlindnessBuffId),
                                    Permanent = true,
                                    IsFromSpell = false,
                                    UseDurationSeconds = false,
                                    DurationValue = new ContextDurationValue
                                    {
                                        Rate = DurationRate.Rounds,
                                        DiceType = DiceType.Zero,
                                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                                        m_IsExtendable = false
                                    }
                                }
                            }
                        }
                    };

                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[]
                        {
                            dmgSelector,
                            blindOnFail
                        }
                    };
                })
                .SetDescriptionValue(
                    "Each creature in the beam is blinded (permanently) and takes 1d3 points " +
                    "of damage per caster level (maximum 16d3). A successful Reflex save negates the blindness and reduces the damage by half. " +
                    "An undead creature caught within the beam takes 1d8 points of damage per caster level (maximum 16d8), " +
                    "or half damage with a successful Reflex save."
                )
                .Configure();
        }
    }
}
