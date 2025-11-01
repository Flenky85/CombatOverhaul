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


namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class EarPiercingScreamAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.EarPiercingScream)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var dmgTrue = (ContextActionDealDamage)root.IfTrue.Actions[0];
                    dmgTrue.Value.DiceType = DiceType.D4;

                    var savedTrue = (ContextActionConditionalSaved)root.IfTrue.Actions[1];
                    var failBuffTrue = (ContextActionApplyBuff)savedTrue.Failed.Actions[0];
                    failBuffTrue.DurationValue.DiceType = DiceType.D2;
                    failBuffTrue.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };
                    failBuffTrue.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var dmgFalse = (ContextActionDealDamage)root.IfFalse.Actions[0];
                    dmgFalse.Value.DiceType = DiceType.D4;

                    var savedFalse = (ContextActionConditionalSaved)root.IfFalse.Actions[1];
                    var failBuffFalse = (ContextActionApplyBuff)savedFalse.Failed.Actions[0];
                    failBuffFalse.DurationValue.DiceType = DiceType.D2;
                    failBuffFalse.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };
                    failBuffFalse.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .EditComponents<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs; 
                    cfg.m_UseMin = false;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 4;    
                },
                cfg => cfg.m_Type == AbilityRankType.DamageDice || cfg.m_Type == AbilityRankType.StatBonus)
                .SetDurationValue("1d2 rounds")
                .SetDescriptionValue(
                    "You unleash a powerful scream, inaudible to all but a single target. The target is dazed for " +
                    "1d2 rounds and takes 1d4 points of sonic damage per caster level (maximum 4d4). A successful " +
                    "Fortitude save negates the daze effect and halves the damage."
                )
                .Configure();
        }
    }
}
