using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level2
{
    [AutoRegister]
    internal static class ArrowOfLawAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ArrowOfLaw)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMax = true;
                    c.m_Max = 6;
                    c.m_Type = AbilityRankType.Default; 
                })

                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var top = (Conditional)c.Actions.Actions[0];

        
                    var condClass = (Conditional)top.IfTrue.Actions[0];
                    var dmg_D6_to_D8 = (ContextActionDealDamage)condClass.IfTrue.Actions[0];
                    dmg_D6_to_D8.Value.DiceType = DiceType.D8;
                    dmg_D6_to_D8.Value.DiceCountValue.ValueRank = AbilityRankType.Default;

                    var dmg_D8_to_D6_A = (ContextActionDealDamage)condClass.IfFalse.Actions[0];
                    dmg_D8_to_D6_A.Value.DiceType = DiceType.D6;

                    var condLawful = (Conditional)top.IfFalse.Actions[0];

                    var dmg_D8_to_D6_B = (ContextActionDealDamage)condLawful.IfFalse.Actions[0];
                    dmg_D8_to_D6_B.Value.DiceType = DiceType.D6;
                })
                .SetDescriptionValue(
                    "You fire a shimmering arrow of pure order from your holy symbol at any one target in range as a " +
                    "ranged touch attack. A chaotic creature struck by an arrow of law takes 1d6 points of damage per " +
                    "caster levels (maximum 6d6). A chaotic outsider instead takes 1d8 points of damage per caster " +
                    "level (maximum 6d8) and is dazed for 1 round. A successful Will save reduces the damage to half and " +
                    "negates the daze effect. This spell deals only half damage to creatures that are neither chaotic nor " +
                    "lawful, and they are not dazed. The arrow has no effect on lawful creatures."
                )
                .Configure();
        }
    }
}
