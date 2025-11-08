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

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class HealAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Heal)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                
                    var root = (Conditional)c.Actions.Actions[0];
                    var heal = (ContextActionHealTarget)root.IfTrue.Actions[2];

                    heal.Value.DiceType = DiceType.D4;
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
                    m_Max = 14,
                    m_AffectedByIntensifiedMetamagic = false
                })
                .SetDescriptionValue(
                    "Heal enables you to channel positive energy into a creature to wipe away injury and afflictions. " +
                    "It immediately ends any and all of the following adverse conditions affecting the target: ability " +
                    "damage, blinded, confused, dazed, dazzled, diseased, exhausted, fatigued, nauseated, poisoned, " +
                    "sickened, and stunned. It also cures 1d4 hit points of damage per level of the caster, to a maximum " +
                    "of 14d4.\n" +
                    "Heal does not remove negative levels or restore permanently drained ability score points.\n" +
                    "If used against an undead creature, heal instead acts like harm."
                )
                .Configure();
        }
    }
}
