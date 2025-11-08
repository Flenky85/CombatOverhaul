using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class SearingLightAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SearingLight)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    if (cfg.m_Type == AbilityRankType.DamageDice)            
                    {
                        cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        cfg.m_Progression = ContextRankProgression.AsIs;   
                        cfg.m_UseMax = true;
                        cfg.m_Max = 8;                             
                    }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var r1 = (ContextActionDealDamage)root.IfTrue.Actions[0];
                    r1.Value.DiceType = DiceType.D10;
                    r1.Value.DiceCountValue.ValueType = ContextValueType.Rank;     
                    r1.Value.DiceCountValue.ValueRank = AbilityRankType.DamageDice;

                    var inner = (Conditional)root.IfFalse.Actions[0];

                    var r2 = (ContextActionDealDamage)inner.IfTrue.Actions[0];
                    r2.Value.DiceType = DiceType.D4;

                    var r3 = (ContextActionDealDamage)inner.IfFalse.Actions[0];
                    r3.Value.DiceType = DiceType.D6;
                })
                .SetDescriptionValue(
                    "Focusing divine power like a ray of the sun, you project a blast of light from your open palm. You must " +
                    "succeed on a ranged touch attack to strike your target. A creature struck by this ray of light takes 1d6 " +
                    "points of divine damage per caster level (maximum 8d6). An undead creature takes 1d10 points of divine damage " +
                    "per caster level (maximum 8d10). A construct creature takes only 1d4 points of damage per caster level " +
                    "(maximum 8d4)."
                )
                .Configure();
        }
    }
}
