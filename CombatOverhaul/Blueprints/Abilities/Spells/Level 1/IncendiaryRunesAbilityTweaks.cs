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
    internal static class IncendiaryRunesAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.IncendiaryRunes)
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
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];

                    dmg.DamageType = new DamageTypeDescription
                    {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Fire
                    };
                    dmg.Value = new ContextDiceValue
                    {
                        DiceType = DiceType.D3,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Rank,
                            ValueRank = AbilityRankType.DamageDice
                        },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                    };
                })
                .SetDescriptionValue(
                    "You trace mystic runes upon a target point. The runes immediately detonate, causing a small " +
                    "surge of flames and dealing 1d3 points of fire damage per caster level (maximum 4d3) to every " +
                    "creature within 10 feet. Those creatures catch fire unless they succeed at Reflex saves. A " +
                    "burning creature takes 1d6 fire damage each round for 1 minute. Each round, it attempts a " +
                    "Reflex save to end the burning."
                )
                .Configure();
        }
    }
}
