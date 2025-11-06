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

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class FearAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Fear)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_UseMax = true;
                    r.m_Max = 10;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = SavingThrowType.Will;
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var dmg = new ContextActionDealDamage
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
                            BonusValue = new ContextValue
                            {
                                ValueType = ContextValueType.Simple,
                                Value = 0
                            }
                        },
                        HalfIfSaved = true,
                        Half = false,
                        IsAoE = false
                    };

                    c.Actions = new ActionList
                    {
                        Actions = new GameAction[] { dmg, saved }
                    };

                    var applyFailed = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    applyFailed.UseDurationSeconds = false;
                    applyFailed.DurationValue.Rate = DurationRate.Rounds;
                    applyFailed.DurationValue.DiceType = DiceType.D2;
                    applyFailed.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    };
                    applyFailed.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                })
                .SetDuration1d2RoundsShared()
                .SetDescriptionValue(
                    "An invisible cone of terror causes each living creature in the area to become frightened unless it succeeds " +
                    "at a Will save. If the Will save succeeds, the creature is shaken for 1 round.\n" +
                    "Additionally, the targets takes 1d4 points of negative energy damage per caster level(maximum 10d4).A successful " +
                    "Will save halves this damage."
                )
                .Configure();
        }
    }
}
