using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class ExplosionOfRotAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ExplosionOfRot)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var svTrue = (ContextActionSavingThrow)root.IfTrue.Actions[1];
                    var dmgTrue = (ContextActionDealDamage)svTrue.Actions.Actions[0];
                    dmgTrue.Value.DiceType = DiceType.D4;
                    dmgTrue.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };

                    var svFalse = (ContextActionSavingThrow)root.IfFalse.Actions[0];
                    var dmgFalse = (ContextActionDealDamage)svFalse.Actions.Actions[0];
                    dmgFalse.Value.DiceType = DiceType.D4;
                    dmgFalse.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 10;                         
                    cfg.m_AffectedByIntensifiedMetamagic = false;
                })
                .SetDescriptionValue(
                    "You call forth a burst of decay that ravages all creatures in the area. Even non-living creatures " +
                    "such as constructs and undead crumble or wither in this malignant eruption of rotting energy. " +
                    "Creatures in the area of effect take 1d4 points of damage per caster level (maximum 10d4) and " +
                    "are staggered for 1d4 rounds. A target that succeeds at a Reflex saving throw takes half damage " +
                    "and negates the staggered effect. Plant creatures are particularly susceptible to this rotting effect; " +
                    "a plant creature caught in the burst suffers a –2 penalty on the saving throw and takes 1 extra point " +
                    "of damage per die."
                )
                .Configure();
        }
    }
}
