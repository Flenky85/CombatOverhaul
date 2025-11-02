using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;


namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class SleepAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Sleep)
                .SetActionType(UnitCommand.CommandType.Standard)   
                .SetIsFullRoundAction(false)
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
                    var root = (Conditional)c.Actions.Actions[0];
                    var cond2 = (Conditional)root.IfFalse.Actions[0];

                    var cond3 = (Conditional)cond2.IfTrue.Actions[0];
                    var saveA = (ContextActionSavingThrow)cond3.IfTrue.Actions[0];
                    InsertSonicDamageAtStart(saveA);
                    SetFailedApplyBuffDurationTo2d3Rounds(saveA);

                    var saveB = (ContextActionSavingThrow)cond2.IfFalse.Actions[0];
                    InsertSonicDamageAtStart(saveB);
                    SetFailedApplyBuffDurationTo2d3Rounds(saveB);

                    static void InsertSonicDamageAtStart(ContextActionSavingThrow save)
                    {
                        var dmg = new ContextActionDealDamage
                        {
                            DamageType = new DamageTypeDescription
                            {
                                Type = DamageType.Energy,
                                Energy = DamageEnergyType.Sonic
                            },
                            Value = new ContextDiceValue
                            {
                                DiceType = DiceType.D3,
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

                        var list = save.Actions.Actions.ToList();
                        list.Insert(0, dmg);
                        save.Actions = new ActionList { Actions = list.ToArray() };
                    }

                    static void SetFailedApplyBuffDurationTo2d3Rounds(ContextActionSavingThrow save)
                    {
                        var cs = save.Actions.Actions.OfType<ContextActionConditionalSaved>().FirstOrDefault();
                        if (cs == null) return;

                        var apply = cs.Failed.Actions.OfType<ContextActionApplyBuff>().FirstOrDefault();
                        if (apply == null) return;

                        apply.DurationValue.Rate = DurationRate.Rounds;
                        apply.DurationValue.DiceType = DiceType.D3;
                        apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                        apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    }
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "A sleep spell causes a magical slumber to come upon 4 HD of creatures, and those who are closest to the " +
                    "spell's point of origin are affected first. Each creature also takes 1d3 points of sonic damage per caster " +
                    "level (maximum 4d3). A successful Will save halves this damage and negates the sleep effect; on a failed save, " +
                    "the creature takes full damage and falls asleep for 2d3 rounds. HD that are not sufficient to affect a creature " +
                    "are wasted. Sleeping creatures are helpless. Wounding awakens an affected creature, but normal noise does not. " +
                    "Sleep does not target unconscious creatures, constructs, or undead creatures."
                )
                .Configure();
        }
    }
}
