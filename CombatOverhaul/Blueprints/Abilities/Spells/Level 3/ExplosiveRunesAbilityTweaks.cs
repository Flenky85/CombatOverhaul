using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
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
    internal static class ExplosiveRunesAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ExplosiveRunes)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 8
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];

                    dmg.DamageType = new DamageTypeDescription
                    {
                        Type = DamageType.Force,
                        Energy = DamageEnergyType.Magic
                    };
                    dmg.Value = new ContextDiceValue
                    {
                        DiceType = DiceType.D4,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageDice
                        },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                    };
                })
                .SetDescriptionValue(
                    "You trace mystical runes upon a target point. The runes immediately detonate, dealing " +
                    "1d4 points of force damage per caster level (maximum 8d4) to every creature within 10 feet."
                )
                .Configure();
        }
    }
}
