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
    internal static class InflictModerateWoundsMasssDamageAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.InflictModerateWoundsMasssDamage)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 14;
                    r.m_AffectedByIntensifiedMetamagic = true;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D3;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDescriptionValue(
                    "Negative energy spreads out in all directions from the point of origin, dealing 1d3 " +
                    "points of damage per caster level (maximum 14d3) to nearby living enemies.\n" +
                    "Like other inflict spells, it cures undead in its area rather than damaging them. " +
                    "A cleric capable of spontaneously casting inflict spells can also spontaneously cast mass inflict spells."
                )
                .Configure();
        }
    }
}
