using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class EyebiteAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Eyebite)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var selfBuff = (ContextActionApplyBuff)c.Actions.Actions[1];
                    selfBuff.UseDurationSeconds = false;
                    selfBuff.DurationSeconds = 0f;
                    selfBuff.DurationValue.Rate = DurationRate.Rounds;
                    selfBuff.DurationValue.DiceType = DiceType.Zero; 
                    selfBuff.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    selfBuff.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 3  
                    };
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var dmgOnSuccess = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.NegativeEnergy
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        Half = true,
                        HalfIfSaved = false,
                        AlreadyHalved = false,
                        IsAoE = false
                    };
                    saved.Succeed.Actions = new GameAction[] { dmgOnSuccess };

                    var dmgOnFail = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.NegativeEnergy
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        Half = false,
                        HalfIfSaved = false,
                        AlreadyHalved = false,
                        IsAoE = false
                    };
                    var failOld = saved.Failed.Actions;
                    var failNew = new GameAction[failOld.Length + 1];
                    failNew[0] = dmgOnFail;
                    Array.Copy(failOld, 0, failNew, 1, failOld.Length);
                    saved.Failed.Actions = failNew;

                    var buff1 = (ContextActionApplyBuff)saved.Failed.Actions[1];
                    buff1.UseDurationSeconds = false;
                    buff1.DurationValue.Rate = DurationRate.Rounds;
                    buff1.DurationValue.DiceType = DiceType.D4;
                    buff1.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    buff1.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var cond1 = (Conditional)saved.Failed.Actions[2];

                    var buff2 = (ContextActionApplyBuff)cond1.IfTrue.Actions[0];
                    buff2.UseDurationSeconds = false;
                    buff2.DurationValue.Rate = DurationRate.Rounds;
                    buff2.DurationValue.DiceType = DiceType.D2;
                    buff2.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };
                    buff2.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var cond2 = (Conditional)cond1.IfTrue.Actions[1];
                    var buff3 = (ContextActionApplyBuff)cond2.IfTrue.Actions[0];
                    buff3.UseDurationSeconds = false;
                    buff3.DurationValue.Rate = DurationRate.Rounds;
                    buff3.DurationValue.DiceType = DiceType.D4;
                    buff3.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    buff3.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "Each round, you can target a single living creature, striking it with waves of power. The target must make a " +
                    "successful Fortitude saving throw, or suffer the spell's harmful effects. Depending on the target's HD, this " +
                    "attack has as many as three effects:\n" +
                    "If the target's HD is 10 or more, it is sickened for 2d4 rounds.\n" +
                    "If the target's HD is 5–9, it is frightened for 1d2 rounds and sickened for 2d4 rounds.\n" +
                    "If the target's HD is 4 or less, it is paralyzed for 2d4 rounds, frightened for 1d2 rounds, and sickened for 2d4 rounds\n" +
                    "You must spend a swift action each round after the first to target a foe.\n" +
                    "Additionally, the targets takes 3d4 points of negative energy. A successful " +
                    "Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
