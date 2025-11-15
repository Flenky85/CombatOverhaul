using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
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
    internal static class FesterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Fester)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 6
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];

                    var failBuff = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    failBuff.DurationValue.Rate = DurationRate.Rounds;
                    failBuff.DurationValue.DiceType = DiceType.D3;
                    failBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    failBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

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
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.DamageDice 
                            },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = true
                    };

                    c.Actions.Actions = new GameAction[] { dmg, saved };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "Necrotic energy permeates the target, blocking healing abilities. The subject gains spell resistance equal to (12 + your caster level) " +
                    "against effects that restore hit points or grant temporary hit points. If the target succeeds on a " +
                    "Fortitude saving throw, fester lasts only a single round. The spell also deals 1d4 " +
                    "points of negative energy damage per caster level (maximum 6d4). On a successful saving throw, " +
                    "the target takes half damage."
                )
                .Configure();
        }
    }
}
