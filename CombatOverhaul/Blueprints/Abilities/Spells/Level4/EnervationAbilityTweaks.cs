using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class EnervationAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Enervation)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_UseMax = true;
                    r.m_Max = 10;                 
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var undeadBuff = (ContextActionApplyBuff)root.IfTrue.Actions[0];
                    undeadBuff.UseDurationSeconds = false;
                    undeadBuff.DurationValue.Rate = DurationRate.Rounds;
                    undeadBuff.DurationValue.DiceType = DiceType.D4;
                    undeadBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    undeadBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var inner = (Conditional)root.IfFalse.Actions[0];

                    var energyDrain = (ContextActionDealDamage)inner.IfFalse.Actions[0];
                    energyDrain.Duration.Rate = DurationRate.Rounds;
                    energyDrain.Duration.DiceType = DiceType.D4;
                    energyDrain.Duration.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    energyDrain.Duration.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var dmgFull = new ContextActionDealDamage
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
                        Half = false
                    };

                    var dmgHalf = new ContextActionDealDamage
                    {
                        DamageType = dmgFull.DamageType,
                        Value = dmgFull.Value,
                        Half = true 
                    };

                    var conditionalSaved = new ContextActionConditionalSaved
                    {
                        Succeed = new ActionList { Actions = new GameAction[] { dmgHalf } },
                        Failed = new ActionList { Actions = new GameAction[] { dmgFull } }
                    };

                    var savingThrow = new ContextActionSavingThrow
                    {
                        Type = SavingThrowType.Fortitude,
                        HasCustomDC = false,
                        CustomDC = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        Actions = new ActionList { Actions = new GameAction[] { conditionalSaved } }
                    };

                    inner.IfFalse.Actions = inner.IfFalse.Actions.Concat(new GameAction[] { savingThrow }).ToArray();
                })
                .SetDuration2d4RoundsShared()
                .SetDescriptionValue(
                    "You point your finger and fire a black ray of negative energy that suppresses the life force of any living " +
                    "creature it strikes. You must make a ranged touch attack to hit. If you hit, the subject gains 1d4 temporary " +
                    "negative levels. Negative levels stack. Assuming the subject survives, it regains lost levels after a number " +
                    "of rounds equal to your caster level (maximum 2d4 rounds). Usually, negative levels have a chance of becoming " +
                    "permanent, but the negative levels from enervation don't last long enough to do so. An undead creature struck " +
                    "by the ray gains 1d4 × 5 temporary hit points for 2d4 rounds.\n" +
                    "Additionally, the target takes 1d4 negative energy damage per caster level(maximum 10d4).A successful saving " +
                    "throw halves the damage."
                )
                .Configure();
        }
    }
}
