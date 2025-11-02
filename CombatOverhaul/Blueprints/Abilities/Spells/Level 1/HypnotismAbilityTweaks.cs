using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;


namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class HypnotismAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Hypnotism)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 4
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    
                    var top = (Conditional)c.Actions.Actions[0];
                    var inner = (Conditional)top.IfFalse.Actions[0];
                    var casterHasFactCond = (Conditional)inner.IfTrue.Actions[0];
                    var savingA = (ContextActionSavingThrow)casterHasFactCond.IfTrue.Actions[0];
                    var savedA = (ContextActionConditionalSaved)savingA.Actions.Actions[0];
                    var applyA = (ContextActionApplyBuff)savedA.Failed.Actions[0];

                    applyA.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.D3,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        m_IsExtendable = true
                    };

                    var dmgA = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Electricity
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D3,
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.DamageDice
                            },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = true,
                        Half = false,
                        IsAoE = false
                    };

                    savingA.Actions = new ActionList { Actions = new GameAction[] { dmgA, savedA } };

                    var savingB = (ContextActionSavingThrow)inner.IfFalse.Actions[0];
                    var savedB = (ContextActionConditionalSaved)savingB.Actions.Actions[0];
                    var applyB = (ContextActionApplyBuff)savedB.Failed.Actions[0];

                    applyB.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.D3,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        m_IsExtendable = true
                    };

                    var dmgB = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Electricity
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D3,
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.DamageDice
                            },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = true,
                        Half = false,
                        IsAoE = false
                    };

                    savingB.Actions = new ActionList { Actions = new GameAction[] { dmgB, savedB } };
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Electricity;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "Your gestures and droning incantation fascinate nearby living creatures, causing them to stop and stare blankly " +
                    "at you in a dazed condition. Roll 2d4 to see how many total HD of creatures you affect. Each creature in the area takes 1d3 electrecity " +
                    "damage per caster level (maximum 4d3)."
                )
                .Configure();
        }
    }
}
