using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level2
{
    [AutoRegister]
    internal static class PoxPustulesAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PoxPustules)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)cond.Failed.Actions[0];

                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D3;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var dmgFull = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Acid
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
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Acid
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        Half = true 
                    };

                    cond.Failed = new ActionList { Actions = new GameAction[] { apply, dmgFull } };
                    cond.Succeed = new ActionList { Actions = new GameAction[] { dmgHalf } };
                })
                .EditComponent<ContextRankConfig>(rc =>
                {
                    rc.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    rc.m_Progression = ContextRankProgression.AsIs;
                    rc.m_UseMax = true;
                    rc.m_Max = 6;
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Fire;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "You inflict a painful, itching rash on the target creature. The target is sickened and takes a –4 " +
                    "penalty to Dexterity (this penalty cannot lower Dexterity below 0).\n " +
                    "Additionally, the target takes 1d4 points of acid damage per caster level(maximum 6d4). " +
                    "A successful Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
