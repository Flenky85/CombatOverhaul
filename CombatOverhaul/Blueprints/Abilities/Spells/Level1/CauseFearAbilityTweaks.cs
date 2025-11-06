using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level1
{
    [AutoRegister]
    internal static class CauseFearAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CauseFear)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond0 = (Conditional)c.Actions.Actions[0];
                    var cond1 = (Conditional)cond0.IfFalse.Actions[0];
                    var save = (ContextActionSavingThrow)cond1.IfTrue.Actions[0];
                    var cs = (ContextActionConditionalSaved)save.Actions.Actions[0];

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

                    static ContextActionDealDamage BuildDamage(bool half)
                    {
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
                                DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank },
                                BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                            },
                            Half = half,            
                            HalfIfSaved = false,    
                            IsAoE = false
                        };
                        return dmg;
                    }

                    var succList = cs.Succeed.Actions.ToList();
                    succList.Insert(0, BuildDamage(true));   
                    cs.Succeed.Actions = succList.ToArray();

                    var failList = cs.Failed.Actions.ToList();
                    failList.Insert(0, BuildDamage(false)); 
                    cs.Failed.Actions = failList.ToArray();
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 4;
                })
                .SetDurationValue("1d2 rounds or 1 round")
                .SetDescriptionValue(
                    "The affected creature takes 1d4 points of damage per caster level (maximum 4d4) " +
                    "and becomes frightened for 1d2 rounds. If the target succeeds at a Will save, it " +
                    "instead takes half damage and is shaken for 1 round. Creatures with 6 or more HD " +
                    "are immune to this effect. Cause fear dispels remove fear."
                )
                .Configure();
        }
    }
}
