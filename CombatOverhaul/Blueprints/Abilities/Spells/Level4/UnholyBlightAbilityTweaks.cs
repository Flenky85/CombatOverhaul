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
    internal static class UnholyBlightAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.UnholyBlight)
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
                    var innerGood = (Conditional)root.IfTrue.Actions[0];
                    
                    var casGoodNotFact = (ContextActionConditionalSaved)innerGood.IfFalse.Actions[0];
                    var dmg1 = (ContextActionDealDamage)casGoodNotFact.Succeed.Actions[0]; dmg1.Value.DiceType = DiceType.D4;
                    var dmg2 = (ContextActionDealDamage)casGoodNotFact.Failed.Actions[0]; dmg2.Value.DiceType = DiceType.D4;

                    var innerNotEvil = (Conditional)root.IfFalse.Actions[0];
                    var casNotEvil = (ContextActionConditionalSaved)innerNotEvil.IfTrue.Actions[0];
                    var dmg3 = (ContextActionDealDamage)casNotEvil.Succeed.Actions[0]; dmg3.Value.DiceType = DiceType.D4;
                    var dmg4 = (ContextActionDealDamage)casNotEvil.Failed.Actions[0]; dmg4.Value.DiceType = DiceType.D4;
                })
                .SetDescriptionValue(
                    "You call up unholy power to smite your enemies. The power takes the form of a cold, cloying miasma of " +
                    "greasy darkness. Only good and neutral (not evil) creatures are harmed by the spell.\n" +
                    "The spell deals 1d4 points of damage per two caster levels(maximum 10d4) to a good creature(or 1d6 per " +
                    "caster level, maximum 10d6, to a good outsider) and causes it to be sickened for 1d4 rounds.A successful " +
                    "Will save reduces damage to half and negates the sickened effect.The effects cannot be negated by remove " +
                    "disease or heal, but remove curse is effective.\n" +
                    "The spell deals only half damage to creatures who are neither evil nor good, and they are not sickened. " +
                    "Such a creature can reduce the damage by half again(down to one - quarter) with a successful Will save."
                )
                .Configure();
        }
    }
}
