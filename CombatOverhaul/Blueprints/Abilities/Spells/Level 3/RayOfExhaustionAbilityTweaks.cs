using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
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
using System;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class RayOfExhaustionAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.RayOfExhaustion)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Type = AbilityRankType.Default;
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 8;
                    r.m_AffectedByIntensifiedMetamagic = true; 
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = c.Actions.Actions;              
                    var condA = (Conditional)root[0];

                    var buffIfTrue = (ContextActionApplyBuff)condA.IfTrue.Actions[0];
                    buffIfTrue.DurationValue.Rate = DurationRate.Rounds;
                    buffIfTrue.DurationValue.DiceType = DiceType.D3;
                    buffIfTrue.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    buffIfTrue.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var condB = (Conditional)condA.IfFalse.Actions[0];
                    var saved = (ContextActionConditionalSaved)condB.IfFalse.Actions[0];

                    var succBuff = (ContextActionApplyBuff)saved.Succeed.Actions[0];
                    succBuff.DurationValue.Rate = DurationRate.Rounds;
                    succBuff.DurationValue.DiceType = DiceType.D3;
                    succBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    succBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var failBuff = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    failBuff.DurationValue.Rate = DurationRate.Rounds;
                    failBuff.DurationValue.DiceType = DiceType.D3;
                    failBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    failBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var negDamage = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.NegativeEnergy
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D6,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = true,
                        IsAoE = false
                    };

                    var newActions = new GameAction[root.Length + 1];
                    newActions[0] = negDamage;
                    Array.Copy(root, 0, newActions, 1, root.Length);
                    c.Actions.Actions = newActions;

                    c.SavingThrowType = SavingThrowType.Fortitude;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "A black ray projects from your pointing finger. You must succeed on a ranged touch attack with the ray to strike a target.\n" +
                    "The subject is immediately exhausted for the spell's duration. A successful Fortitude save means the creature is only fatigued.\n" +
                    "A character that is already fatigued also becomes exhausted.\n" + 
                    "This spell has no effect on a creature that is already exhausted. Unlike normal exhaustion or fatigue, the effect ends as soon " +
                    "as the spell's duration expires.\n" +
                    "Additionally, the spell deals 1d6 points of negative energy damage per caster level(maximum 8d6); on a successful save, the damage is halved."
                )
                .Configure();
        }
    }
}
