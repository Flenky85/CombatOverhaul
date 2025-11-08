using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class HarmDamageAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HarmDamage)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D4;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                    dmg.Value.BonusValue = new ContextValue
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
