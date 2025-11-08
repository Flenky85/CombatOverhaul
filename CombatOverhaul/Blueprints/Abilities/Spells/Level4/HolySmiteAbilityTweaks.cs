using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class HolySmiteAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HolySmite)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    if (cfg.m_Type == AbilityRankType.DamageDice)
                    {
                        cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        cfg.m_Progression = ContextRankProgression.AsIs;
                        cfg.m_UseMax = true;
                        cfg.m_Max = 10;
                    }
                })
                .EditComponent<ContextCalculateSharedValue>(sv =>
                {
                    if (sv.ValueType == AbilitySharedValue.Damage)
                    {
                        sv.Value.DiceType = DiceType.D4;
                        sv.Value.DiceCountValue.ValueType = ContextValueType.Rank;
                        sv.Value.DiceCountValue.ValueRank = AbilityRankType.DamageDice;
                    }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];
                    var innerEvil = (Conditional)root.IfTrue.Actions[0];
                    var casEvilIfFalse = (ContextActionConditionalSaved)innerEvil.IfFalse.Actions[0];

                    var dmg1 = (ContextActionDealDamage)casEvilIfFalse.Succeed.Actions[0];
                    dmg1.Value.DiceType = DiceType.D4;

                    var dmg2 = (ContextActionDealDamage)casEvilIfFalse.Failed.Actions[0];
                    dmg2.Value.DiceType = DiceType.D4;

                    var innerNotGood = (Conditional)root.IfFalse.Actions[0];
                    var casNotGood = (ContextActionConditionalSaved)innerNotGood.IfTrue.Actions[0];

                    var dmg3 = (ContextActionDealDamage)casNotGood.Succeed.Actions[0];
                    dmg3.Value.DiceType = DiceType.D4;

                    var dmg4 = (ContextActionDealDamage)casNotGood.Failed.Actions[0];
                    dmg4.Value.DiceType = DiceType.D4;
                })
                .SetDescriptionValue(
                    "You draw down holy power to smite your enemies. Only evil and neutral creatures " +
                    "are harmed by the spell; good creatures are unaffected.\n" +
                    "The spell deals 1d4 points of damage per two caster levels(maximum 10d4) to each evil " +
                    "creature in the area(or 1d6 points of damage per caster level, maximum 10d6, to an " +
                    "evil outsider) and causes it to become blinded for 1 round.A successful Will saving " +
                    "throw reduces damage to half and negates the blinded effect.\n" +
                    "The spell deals only half damage to creatures who are neither good nor evil, and " +
                    "they are not blinded.Such a creature can reduce that damage by half(down to one - " +
                    "quarter of the roll) with a successful Will save."
                )
                .Configure();
        }
    }
}
