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
    internal static class VampiricTouchAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.VampiricTouch)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Type = AbilityRankType.DamageDice;
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.Div2; 
                    r.m_UseMax = true;
                    r.m_Max = 4;                                   
                    r.m_AffectedByIntensifiedMetamagic = false;    
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var actions = c.Actions.Actions;
                    var cond = (Conditional)actions[1];
                    var dmgTrue = (ContextActionDealDamage)cond.IfTrue.Actions[0];
                    dmgTrue.Value.DiceType = DiceType.D8;

                    var dmgFalse = (ContextActionDealDamage)cond.IfFalse.Actions[0];
                    dmgFalse.Value.DiceType = DiceType.D8;

                    var buff = (ContextActionApplyBuff)actions[2];
                    buff.DurationValue.Rate = DurationRate.Rounds;
                    buff.DurationValue.DiceType = DiceType.Zero;
                    buff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    buff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "You must succeed on a melee touch attack. Your touch deals 1d8 points of damage per two caster levels" +
                    " (maximum 4d8). You gain temporary hit points equal to the damage you deal. The temporary hit points " +
                    "disappear 6 rounds later."
                )
                .Configure();
        }
    }
}
