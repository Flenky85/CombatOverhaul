using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level1
{
    [AutoRegister]
    internal static class TouchOfGracelessnessAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TouchOfGracelessness)
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
                    var applyBuff = (ContextActionApplyBuff)c.Actions.Actions[0];

                    applyBuff.DurationValue.Rate = DurationRate.Rounds;
                    applyBuff.DurationValue.DiceType = DiceType.D3;
                    applyBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    applyBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyBuff.DurationValue.m_IsExtendable = true;

                    var dealDamage = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Direct
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
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

                    var saved = new ContextActionConditionalSaved
                    {
                        Succeed = new ActionList { Actions = System.Array.Empty<GameAction>() },
                        Failed = new ActionList { Actions = new GameAction[] { applyBuff } }
                    };

                    c.Actions.Actions = new GameAction[] { dealDamage, saved };
                    c.SavingThrowType = SavingThrowType.Fortitude;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "A coruscating ray springs from your hand. You must succeed on a ranged touch attack to strike a target. " +
                    "On a hit, the target takes 1d4 points of negative energy damage per caster level, maximum 4d4. " +
                    "The target then attempts a Fortitude save; on a success, it takes half damage and the spell applies no Dexterity penalty. " +
                    "On a failure, the target takes full damage and also takes a penalty to Dexterity equal to 1d6 plus 1 per two caster levels, maximum 1d6+5. " +
                    "The subject's Dexterity score cannot drop below 1. This penalty does not stack with itself; apply the highest penalty instead. " +
                    "The Dexterity penalty lasts for 2d3 rounds."
                )
                .Configure();
        }
    }
}
