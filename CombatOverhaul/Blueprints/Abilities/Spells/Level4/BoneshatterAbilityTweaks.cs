using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
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
    internal static class BoneshatterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Boneshatter)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_UseMax = true;
                    c.m_Max = 10;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.DamageType = new DamageTypeDescription
                    {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Unholy
                    };
                    dmg.Value = new ContextDiceValue
                    {
                        DiceType = DiceType.D6,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.Default
                        },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                    };
                    dmg.HalfIfSaved = true;
                    c.Actions.Actions[0] = dmg;

                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[1];

                    var applyOnSucceed = (ContextActionApplyBuff)saved.Succeed.Actions[0];
                    applyOnSucceed.DurationValue.Rate = DurationRate.Rounds;
                    applyOnSucceed.DurationValue.DiceType = DiceType.D3;
                    applyOnSucceed.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    applyOnSucceed.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    saved.Succeed.Actions[0] = applyOnSucceed;

                    var applyOnFailed = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    applyOnFailed.DurationValue.Rate = DurationRate.Rounds;
                    applyOnFailed.DurationValue.DiceType = DiceType.D3;
                    applyOnFailed.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    applyOnFailed.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    saved.Failed.Actions[0] = applyOnFailed;

                    c.Actions.Actions[1] = saved;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "The target's bones (or exoskeleton) splinter, dealing 1d6 points of damage per caster level (maximum 10d6) to the target, " +
                    "which is also exhausted for 2d3 rounds. If the target succeeds at its Fortitude save, it takes half damage and is fatigued " +
                    "for 2d3 rounds instead."
                )
                .Configure();
        }
    }
}
