using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
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

namespace CombatOverhaul.Blueprints.Abilities.Hellknight
{
    [AutoRegister]
    internal static class HellKnightOrderOfTheRackAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HellKnightOrderOfTheRackAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saving = (ContextActionSavingThrow)c.Actions.Actions[0];
                    saving.Type = SavingThrowType.Will;

                    var cond = (ContextActionConditionalSaved)saving.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)cond.Failed.Actions[0];

                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D3;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.m_IsExtendable = false;

                    var dmgFull = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank, ValueRank = AbilityRankType.DamageDice },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = false,
                        AlreadyHalved = false,
                        IsAoE = false,
                        IgnoreCritical = true
                    };
                    var dmgHalf = new ContextActionDealDamage
                    {
                        DamageType = dmgFull.DamageType,
                        Value = dmgFull.Value,
                        Half = true,               
                        HalfIfSaved = false,
                        AlreadyHalved = false,
                        IsAoE = false,
                        IgnoreCritical = true
                    };

                    cond.Succeed.Actions = new GameAction[] { dmgHalf };
                    cond.Failed.Actions = new GameAction[] { dmgFull, apply };
                })
                .AddContextRankConfig(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.ClassLevel,
                    m_Class = new BlueprintCharacterClassReference[]
                  {
                    BlueprintTool.GetRef<BlueprintCharacterClassReference>("ed246f1680e667b47b7427d51e651059") 
                  },
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 10
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "As a standard action you can target a single creature. That creature must pass a Will saving throw " +
                    "(DC 10 + level in this prestige class + your Charisma bonus) or become unable to cast spells for 2d3 " +
                    "rounds. It also deals 1d4 points of damage per level in this prestige class, reduced to half on a successful Will saving throw." +
                    " The same effect occurs whenever you score a critical hit.\n" +
                    "This ability consumes 3 charges; the Hellknight begins with 3 charges, gains 1 additional charge " +
                    "at 5th level, and gains 1 more at 9th level. Recover 1 charge each round."
                )
                .Configure();
        }
    }
}
