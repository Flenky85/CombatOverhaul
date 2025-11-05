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

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ExcruciatingDeformationAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ExcruciatingDeformation)
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
                    var original = c.Actions.Actions;
                    var saved = (ContextActionConditionalSaved)original[0];
                    var fireDamage = new ContextActionDealDamage
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
                        HalfIfSaved = true,
                    };

                    c.Actions.Actions = new GameAction[] { fireDamage, saved };

                    var failBuff = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    failBuff.DurationValue.Rate = DurationRate.Rounds;
                    failBuff.DurationValue.DiceType = DiceType.D3;
                    failBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    failBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "Your touch attack causes your target to become painfully malformed. Its limbs twist and buckle, while its body " +
                    "contorts uncontrollably, shifting and warping. Each round the target suffers excruciating pain and takes 2d6 " +
                    "points of direct damage, 1 point of Dexterity damage, and 1 point of Constitution damage, and its speed is reduced " +
                    "by 10 feet. Once per round as a free action on its turn, the target can attempt a new Fortitude saving throw to " +
                    "resist the spell for 1 round.\n" +
                    "When you hit with the touch attack, it also deals 1d3 points of negative energy " +
                    "damage per caster level (maximum 8d3); a successful Fortitude saving throw halves this damage."
                )
                .Configure();
        }
    }
}
