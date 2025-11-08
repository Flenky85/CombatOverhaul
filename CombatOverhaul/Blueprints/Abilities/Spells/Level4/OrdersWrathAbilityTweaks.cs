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
    internal static class OrdersWrathAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.OrdersWrath)
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
                    var innerChaotic = (Conditional)root.IfTrue.Actions[0];
                    var casChaoticNotFact = (ContextActionConditionalSaved)innerChaotic.IfFalse.Actions[0];

                    var dmg1 = (ContextActionDealDamage)casChaoticNotFact.Succeed.Actions[0];
                    dmg1.Value.DiceType = DiceType.D4;

                    var dmg2 = (ContextActionDealDamage)casChaoticNotFact.Failed.Actions[0];
                    dmg2.Value.DiceType = DiceType.D4;

                    var innerNotLawful = (Conditional)root.IfFalse.Actions[0];
                    var casNotLawful = (ContextActionConditionalSaved)innerNotLawful.IfTrue.Actions[0];

                    var dmg3 = (ContextActionDealDamage)casNotLawful.Succeed.Actions[0];
                    dmg3.Value.DiceType = DiceType.D4;

                    var dmg4 = (ContextActionDealDamage)casNotLawful.Failed.Actions[0];
                    dmg4.Value.DiceType = DiceType.D4;
                })
                .SetDescriptionValue(
                    "You channel lawful power to smite enemies. The power takes the form of a three-dimensional " +
                    "grid of energy. Only chaotic and neutral (not lawful) creatures are harmed by the spell.\n" +
                    "The spell deals 1d4 points of damage per two caster levels (maximum 10d4) to chaotic creatures " +
                    "(or 1d6 points of damage per caster level, maximum 10d6, to chaotic outsiders) and causes them " +
                    "to be dazed for 1 round. A successful Will save reduces the damage to half and negates the daze effect.\n" +
                    "The spell deals only half damage to creatures who are neither chaotic nor lawful, and they are not dazed. " +
                    "They can reduce the damage in half again (down to one-quarter of the roll) with a successful Will save."
                )
                .Configure();
        }
    }
}
