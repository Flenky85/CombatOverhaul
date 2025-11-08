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
    internal static class ChaosHammerAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ChaosHammer)
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

                    var innerLawful = (Conditional)root.IfTrue.Actions[0];
                    var casLawfulNotFact = (ContextActionConditionalSaved)innerLawful.IfFalse.Actions[0];

                    var dmg1 = (ContextActionDealDamage)casLawfulNotFact.Succeed.Actions[0];
                    dmg1.Value.DiceType = DiceType.D4;

                    var dmg2 = (ContextActionDealDamage)casLawfulNotFact.Failed.Actions[0]; 
                    dmg2.Value.DiceType = DiceType.D4;

                    var innerNotChaotic = (Conditional)root.IfFalse.Actions[0];
                    var casNotChaotic = (ContextActionConditionalSaved)innerNotChaotic.IfTrue.Actions[0];

                    var dmg3 = (ContextActionDealDamage)casNotChaotic.Succeed.Actions[0];    
                    dmg3.Value.DiceType = DiceType.D4;

                    var dmg4 = (ContextActionDealDamage)casNotChaotic.Failed.Actions[0];     
                    dmg4.Value.DiceType = DiceType.D4;
                })
                .SetDescriptionValue(
                    "You unleash chaotic power to smite your enemies. The power takes the form of a multicolored " +
                    "explosion of leaping, ricocheting energy. Only lawful and neutral (not chaotic) creatures are " +
                    "harmed by the spell.\n" +
                    "The spell deals 1d4 points of damage per two caster levels(maximum 10d4) to lawful creatures" +
                    "(or 1d6 points of damage per caster level, maximum 10d6, to lawful outsiders) and slows them " +
                    "for 1d6 rounds(see the slow spell).A successful Will save reduces the damage by half and " +
                    "negates the slow effect.\n" +
                    "The spell deals only half damage against creatures who are neither lawful nor chaotic, and " +
                    "they are not slowed.Such a creature can reduce the damage by half again(down to one - quarter) " +
                    "with a successful Will save."
                )
                .Configure();
        }
    }
}
