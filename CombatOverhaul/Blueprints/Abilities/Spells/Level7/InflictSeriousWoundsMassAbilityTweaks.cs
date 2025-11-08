using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class InflictSeriousWoundsMassAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.InflictSeriousWoundsDamage)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var condA = (Conditional)c.Actions.Actions[0];
                    var condB = (Conditional)condA.IfFalse.Actions[0];
                    var condC = (Conditional)condB.IfTrue.Actions[0];
                    var heal = (ContextActionHealTarget)condC.IfTrue.Actions[0];

                    heal.Value.DiceType = DiceType.D3;
                    heal.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                    heal.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_UseMax = true,
                    m_Max = 16,
                    m_AffectedByIntensifiedMetamagic = false
                })
                .SetDescriptionValue(
                    "Negative energy spreads out in all directions from the point of origin, dealing 1d3 " +
                    "points of damage per caster level (maximum 16d3) to nearby living enemies.\n" +
                    "Like other inflict spells, it cures undead in its area rather than damaging them. " +
                    "A cleric capable of spontaneously casting inflict spells can also spontaneously cast mass inflict spells."
                )
                .Configure();
        }
    }
}
