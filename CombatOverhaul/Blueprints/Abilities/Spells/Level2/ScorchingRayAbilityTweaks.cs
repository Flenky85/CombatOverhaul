using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
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
    internal static class ScorchingRayAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ScorchingRay)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D8;
                    deal.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.StartPlusDivStep;
                    cfg.m_StartLevel = 0;  
                    cfg.m_StepLevel = 3;   
                    cfg.m_UseMin = true;
                    cfg.m_Min = 1;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 3;
                })
                .SetDescriptionValue(
                    "You blast your enemies with a searing beam of fire. You fire one ray, plus one additional " +
                    "ray every 3 caster levels (maximum 3 rays at caster level 6). Each ray requires a ranged " +
                    "touch attack to hit and deals 2d8 fire damage."
                )
                .Configure();
        }
    }
}
