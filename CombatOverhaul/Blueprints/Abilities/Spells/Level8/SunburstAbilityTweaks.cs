using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class SunburstAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Sunburst)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.Actions.Actions[0];
                    var dmgPerLevel = (ContextActionDealDamage)cond.IfTrue.Actions[0];
                    dmgPerLevel.Value.DiceType = DiceType.D8;
                })
                .EditComponent<ContextRankConfig>(r =>
                {
                    if (r.m_Type == AbilityRankType.Default && r.m_BaseValueType == ContextRankBaseValueType.CasterLevel)
                    {
                        r.m_UseMax = true;
                        r.m_Max = 18; 
                        r.m_Progression = ContextRankProgression.AsIs; 
                    }
                })
                .SetDescriptionValue(
                    "Sunburst causes a globe of searing radiance to explode silently from a point you select. " +
                    "All creatures in the globe are blinded and take 6d6 points of damage. A successful Reflex " +
                    "save negates the blindness and reduces the damage by half.\n" +
                    "An undead creature caught within the globe takes 1d8 points of damage per caster level (maximum 18d8), " +
                    "or half damage with a successful Reflex save."
                )
                .Configure();
        }
    }
}
