using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class HolyWhisperAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HolyWhisper)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 10;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var c0 = (Conditional)c.Actions.Actions[0];
                    var svUndead = (ContextActionSavingThrow)c0.IfTrue.Actions[0];
                    var savedUndead = (ContextActionConditionalSaved)svUndead.Actions.Actions[0];
                    var dmgUndead = (ContextActionDealDamage)savedUndead.Failed.Actions[0];
                    dmgUndead.Value.DiceType = DiceType.D6;
                    dmgUndead.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank, ValueRank = AbilityRankType.Default };
                    dmgUndead.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var condGoodEvil = (Conditional)c0.IfFalse.Actions[0];
                    var condEvil = (Conditional)condGoodEvil.IfFalse.Actions[0];
                    var svEvil = (ContextActionSavingThrow)condEvil.IfTrue.Actions[0];
                    var savedEvil = (ContextActionConditionalSaved)svEvil.Actions.Actions[0];

                    var applyDebuff = (ContextActionApplyBuff)savedEvil.Failed.Actions[0];
                    applyDebuff.UseDurationSeconds = false;
                    applyDebuff.DurationValue.Rate = DurationRate.Rounds;
                    applyDebuff.DurationValue.DiceType = DiceType.D3;
                    applyDebuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    applyDebuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyDebuff.DurationValue.m_IsExtendable = false;

                    var condFeat = (Conditional)savedEvil.Failed.Actions[1];
                    var dmgFeat = (ContextActionDealDamage)condFeat.IfTrue.Actions[0];
                    dmgFeat.Value.DiceType = DiceType.D6;
                    dmgFeat.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank, ValueRank = AbilityRankType.Default };
                    dmgFeat.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var condDualFacts = (Conditional)condFeat.IfFalse.Actions[0];
                    var dmgDual = (ContextActionDealDamage)condDualFacts.IfTrue.Actions[0];
                    dmgDual.Value.DiceType = DiceType.D6;
                    dmgDual.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank, ValueRank = AbilityRankType.Default };
                    dmgDual.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDescriptionValue(
                    "You whisper a single word in the primordial language of good that is anathema to the minions of evil " +
                    "and strengthens the resolve of good creatures. Evil creatures within the burst must make a Fortitude " +
                    "saving throw or become sickened for 2d3 rounds. Evil outsiders with the evil subtype, evil-aligned " +
                    "dragons, and undead in the burst also take 1d6 points of damage per caster level (10d6 maximun) if they fail their saves. Good-aligned " +
                    "creatures in the burst gain a +2 sacred bonus on attack and damage rolls for 1 round."
                )
                .Configure();
        }
    }
}
