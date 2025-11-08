using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
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

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class PoisonAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Poison)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 8
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Acid
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D6,
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
                        IsAoE = false
                    };

                    var oldActions = c.Actions.Actions;
                    var newActions = new GameAction[oldActions.Length + 1];
                    newActions[0] = dmg;
                    Array.Copy(oldActions, 0, newActions, 1, oldActions.Length);
                    c.Actions.Actions = newActions;
                    c.SavingThrowType = SavingThrowType.Fortitude;
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Fire;
                })
                .SetDescriptionValue(
                    "Calling upon the venomous powers of natural predators, you infect the subject with a horrible " +
                    "poison by making a successful melee touch attack. This poison deals 1d3 Constitution damage per " +
                    "round for 6 rounds. Poisoned creatures can make a Fortitude save each round to negate the damage " +
                    "and end the affliction.\n" +
                    "Additionally, the target takes 1d6 points of acid damage per caster level(maximum 8d6). " +
                    "A successful Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
