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
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ScareAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Scare)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond0 = (Conditional)c.Actions.Actions[0];               
                    var cond1 = (Conditional)cond0.IfFalse.Actions[0];           

                    var condA0 = (Conditional)cond1.IfTrue.Actions[0];           
                    var condA1 = (Conditional)condA0.IfTrue.Actions[0];          
                    var saveA = (ContextActionSavingThrow)condA1.IfFalse.Actions[0];
                    var csA = (ContextActionConditionalSaved)saveA.Actions.Actions[0];

                    var condB0 = (Conditional)cond1.IfFalse.Actions[0];          
                    var saveB = (ContextActionSavingThrow)condB0.IfFalse.Actions[0];
                    var csB = (ContextActionConditionalSaved)saveB.Actions.Actions[0];

                    static ContextActionDealDamage BuildDamage(bool half) => new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.NegativeEnergy
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D3,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank }, 
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        Half = half,
                        HalfIfSaved = false,
                        IsAoE = false
                    };

                    static void PatchCS(ContextActionConditionalSaved cs)
                    {
                        var succBuff = (ContextActionApplyBuff)cs.Succeed.Actions[0];
                        succBuff.DurationValue.Rate = DurationRate.Rounds;
                        succBuff.DurationValue.DiceType = DiceType.Zero;
                        succBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                        succBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };

                        var failBuff = (ContextActionApplyBuff)cs.Failed.Actions[0];
                        failBuff.DurationValue.Rate = DurationRate.Rounds;
                        failBuff.DurationValue.DiceType = DiceType.D2;
                        failBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };
                        failBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                        var succList = cs.Succeed.Actions.ToList();
                        succList.Insert(0, BuildDamage(true));
                        cs.Succeed.Actions = succList.ToArray();

                        var failList = cs.Failed.Actions.ToList();
                        failList.Insert(0, BuildDamage(false));
                        cs.Failed.Actions = failList.ToArray();
                    }

                    PatchCS(csA);
                    PatchCS(csB);
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Type = AbilityRankType.Default;
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 6;
                })
                .SetDurationValue("1d2 rounds or 1 round")
                .SetDescriptionValue(
                    "The affected creatures takes 1d3 points of damage per caster level (maximum 6d3) " +
                    "and becomes frightened for 1d2 rounds. If the target succeeds at a Will save, it " +
                    "instead takes half damage and is shaken for 1 round. Creatures with 6 or more HD " +
                    "are immune to this effect. Scare dispels remove fear."
                )
                .Configure();
        }
    }
}
