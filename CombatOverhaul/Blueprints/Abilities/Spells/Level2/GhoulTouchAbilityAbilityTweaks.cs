using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
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

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level2
{
    [AutoRegister]
    internal static class GhoulTouchAbilityAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.GhoulTouchAbility)
                .AddContextRankConfig(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 6
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saving = (ContextActionSavingThrow)c.Actions.Actions[0];
                    var cond = (ContextActionConditionalSaved)saving.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)cond.Failed.Actions[0];

                    var extendable = apply.DurationValue.m_IsExtendable;
                    apply.DurationValue = new ContextDurationValue
                    {
                        m_IsExtendable = extendable,
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.D2,
                        DiceCountValue = ContextValues.Constant(1),
                        BonusValue = ContextValues.Constant(0)
                    };

                    var damageFull = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Acid
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.DamageDice
                            },
                            BonusValue = new ContextValue()
                        },
                        Half = false,
                        IsAoE = false
                    };

                    var damageHalf = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Acid
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.DamageDice
                            },
                            BonusValue = new ContextValue()
                        },
                        Half = true,   
                        IsAoE = false
                    };

                    cond.Succeed = new ActionList
                    {
                        Actions = new GameAction[] { damageHalf }
                    };
                    cond.Failed = new ActionList
                    {
                        Actions = new GameAction[] { damageFull, apply }
                    };

                })
                .SetDescriptionValue(
                    "Imbuing you with negative energy, this spell allows you to paralyze a single living humanoid " +
                    "for the duration of the spell with a successful melee touch attack. A paralyzed subject exudes " +
                    "a carrion stench that causes all living creatures (except you) in a 10-foot-radius spread to " +
                    "become sickened (Fortitude negates). A neutralize poison spell removes the effect from a sickened " +
                    "creature, and creatures immune to poison are unaffected by the stench. This is a poison effect.\n" +
                    "Additionally, on a hit the target takes 1d4 acid damage per caster level (maximum 6d4). " +
                    "A successful Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
