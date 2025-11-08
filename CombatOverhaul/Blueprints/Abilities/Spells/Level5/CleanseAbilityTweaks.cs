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

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class CleanseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Cleanse)
                .EditComponent<ContextRankConfig>(r =>
                {
                    if (r.m_Type == AbilityRankType.Default)
                    {
                        r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        r.m_Progression = ContextRankProgression.AsIs;
                        r.m_UseMax = true;
                        r.m_Max = 12;
                    }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var heal = (ContextActionHealTarget)c.Actions.Actions[0];
                    heal.Value.DiceType = DiceType.D4;
                    heal.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    heal.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDescriptionValue(
                    "This spell cures 1d4 points of damage per caster level (maximum 12d4) and ends any " +
                    "and all of the following adverse conditions affecting you: ability damage, blinded, confused, " +
                    "dazzled, diseased, exhausted, fatigued, nauseated, poisoned, and sickened."
                )
                .Configure();
        }
    }
}
