using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class HealMassDamageAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HealMassDamage)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 20;
                    cfg.m_AffectedByIntensifiedMetamagic = false;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D3;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDescriptionValue(
                    "You channel positive energy to cure 1d3 points of damage per caster level (maximum 20d3) on " +
                    "each creature in the radius. Like other cure spells, deals damage to " +
                    "undead in its area rather than curing them. An undead creature can apply spell resistance, " +
                    "and can attempt a Will save to take half damage."
                )
                .Configure();
        }
    }
}
