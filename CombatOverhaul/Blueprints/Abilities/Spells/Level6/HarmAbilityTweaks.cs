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
    internal static class HarmAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Harm)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];
                    var heal = (ContextActionHealTarget)root.IfTrue.Actions[0];

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
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Type = AbilityRankType.DamageDice;
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 14;
                    r.m_AffectedByIntensifiedMetamagic = false;
                })
                .SetDescriptionValue(
                    "Harm charges a subject with negative energy that deal 1d4 points of damage per caster level (to a maximum of 14d4). " +
                    "If the creature makes a successful Will save, harm deals half this amount.\n" +
                    "If used on an undead creature, harm acts like heal."
                )
                .Configure();
        }
    }
}
