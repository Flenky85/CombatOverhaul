using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
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
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class RayOfEnfeeblementAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.RayOfEnfeeblement)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = SavingThrowType.Fortitude;

                    var apply = c.Actions.Actions.OfType<ContextActionApplyBuff>().FirstOrDefault();
                    if (apply != null)
                    {
                        apply.DurationValue.Rate = DurationRate.Rounds;
                        apply.DurationValue.DiceType = DiceType.D3;
                        apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                        apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    }

                    var damage = new ContextActionDealDamage
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
                        Failed = new ActionList
                        {
                            Actions = apply != null
                                ? new GameAction[] { apply }
                                : System.Array.Empty<GameAction>()
                        }
                    };

                    c.Actions.Actions = new GameAction[] { damage, saved };
                })
                .AddComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Type = AbilityRankType.DamageDice;
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 4;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "A coruscating ray springs from your hand. You must succeed on a ranged touch attack to strike a target. " +
                    "On a hit, the target takes 1d4 points of negative energy damage per caster level, maximum 4d4. " +
                    "The target then attempts a Fortitude save; on a success, it takes half damage and the spell applies no Strength penalty. " +
                    "On a failure, the target takes full damage and also takes a penalty to Strength equal to 1d6 plus 1 per two caster levels, maximum 1d6+5. " +
                    "The subject's Strength score cannot drop below 1. This penalty does not stack with itself; apply the highest penalty instead. " +
                    "The Strength penalty lasts for 2d3 rounds."
                )
                .Configure();
        }
    }
}
