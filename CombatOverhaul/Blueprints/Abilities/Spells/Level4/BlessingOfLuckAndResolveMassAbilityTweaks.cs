using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class BlessingOfLuckAndResolveMassAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BlessingOfLuckAndResolveMass)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond1 = (Conditional)c.Actions.Actions[0];
                    var cond2 = (Conditional)cond1.IfFalse.Actions[0];
                    var cond3 = (Conditional)cond2.IfTrue.Actions[0];
                    var applyA = (ContextActionApplyBuff)cond3.IfTrue.Actions[0];
                    var applyB = (ContextActionApplyBuff)cond2.IfFalse.Actions[0];

                    applyA.UseDurationSeconds = false;
                    applyA.DurationValue.Rate = DurationRate.Rounds;
                    applyA.DurationValue.DiceType = DiceType.Zero;
                    applyA.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    applyA.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };
                    applyA.DurationValue.m_IsExtendable = true;

                    applyB.UseDurationSeconds = false;
                    applyB.DurationValue.Rate = DurationRate.Rounds;
                    applyB.DurationValue.DiceType = DiceType.Zero;
                    applyB.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    applyB.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };
                    applyB.DurationValue.m_IsExtendable = true;
                })
                .SetDuration6RoundsShared()
                .Configure();

            
        }
    }
}
