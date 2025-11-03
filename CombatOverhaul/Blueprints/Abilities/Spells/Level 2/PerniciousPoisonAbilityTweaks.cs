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
using Kingmaker.Utility;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;
using static Pathfinding.Util.RetainedGizmos;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class PerniciousPoisonAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PerniciousPoison)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Type = AbilityRankType.Default;
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 6;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var applyBuff = (ContextActionApplyBuff)c.Actions.Actions[0];

                    applyBuff.DurationValue.Rate = DurationRate.Rounds;
                    applyBuff.DurationValue.DiceType = DiceType.D4;
                    applyBuff.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    applyBuff.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var acidDamage = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Acid
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

                    var saveGate = new ContextActionSavingThrow
                    {
                        Type = SavingThrowType.Fortitude,
                        Actions = new ActionList
                        {
                            Actions = new GameAction[]
                            {
                                acidDamage,
                                new ContextActionConditionalSaved
                                {
                                    Succeed = new ActionList { Actions = new GameAction[0] },
                                    Failed  = new ActionList { Actions = new GameAction[] { applyBuff } }
                                }
                            }
                        }
                    };

                    c.Actions = new ActionList { Actions = new GameAction[] { saveGate } };
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Acid;
                })
                .SetLocalizedSavingThrow(SavingThrow.FortPartial)
                .SetDuration2d4RoundsShared()
                .SetDescriptionValue(
                    "You weaken the target's defenses against poison. The target gains a –4 penalty on saves against " +
                    "poison. Attempts to cure the poisoned target take a –4 penalty.\n" +
                    "Additionally, this spell deals 1d6 acid damage per caster level(maximum 6d6).A successful Fortitude " +
                    "save halves this damage and prevents the debuff from being applied.On a failed save, the debuff is " +
                    "applied for 2d4 rounds."
                )
                .Configure();
        }
    }
}
