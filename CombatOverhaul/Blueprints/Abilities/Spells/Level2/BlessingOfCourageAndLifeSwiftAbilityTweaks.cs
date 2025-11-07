using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level2
{
    [AutoRegister]
    internal static class BlessingOfCourageAndLifeSwiftAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BlessingOfCourageAndLifeSwift)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var heal = (ContextActionHealTarget)c.Actions.Actions[1];
                    heal.Value.DiceType = DiceType.D2;
                    heal.Value.DiceCountValue = ContextValues.Rank();    
                    heal.Value.BonusValue = ContextValues.Constant(0);   
                })
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMax = true;
                    c.m_Max = 6;
                })
                .Configure();
        }
    }
}
