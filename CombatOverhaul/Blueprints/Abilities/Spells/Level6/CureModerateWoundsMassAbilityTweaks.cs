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
    internal static class CureModerateWoundsMassAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CureModerateWoundsMass)
                .EditComponent<ContextRankConfig>(r =>
                {
                    if (r.m_Type == AbilityRankType.Default)
                    {
                        r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        r.m_Progression = ContextRankProgression.AsIs;
                        r.m_UseMax = true;
                        r.m_Max = 14; 
                    }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];
                    var condIsAlly = (Conditional)root.IfFalse.Actions[0];                
                    var condTargetUndead = (Conditional)condIsAlly.IfTrue.Actions[0];     
                    var heal = (ContextActionHealTarget)condTargetUndead.IfFalse.Actions[0];

                    heal.Value.DiceType = DiceType.D3;
                    heal.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default  
                    };
                    heal.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0                           
                    };
                })
                .SetDescriptionValue(
                    "You channel positive energy to cure 1d3 points of damage per caster level (maximum 14d3) on " +
                    "each creature in the radius. Like other cure spells, mass cure light wounds deals damage to " +
                    "undead in its area rather than curing them. An undead creature can apply spell resistance, " +
                    "and can attempt a Will save to take half damage."
                )
                .Configure();
        }
    }
}
