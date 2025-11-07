using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class OverwhelmingPresenceAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.OverwhelmingPresence)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (ContextActionConditionalSaved)c.Actions.Actions[0];

                    var buffOnSuccess = (ContextActionApplyBuff)cond.Succeed.Actions[0];
                    buffOnSuccess.UseDurationSeconds = false;
                    buffOnSuccess.DurationValue.Rate = DurationRate.Rounds;
                    buffOnSuccess.DurationValue.DiceType = DiceType.D3;
                    buffOnSuccess.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    buffOnSuccess.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var buffOnFail = (ContextActionApplyBuff)cond.Failed.Actions[0];
                    buffOnFail.UseDurationSeconds = false;
                    buffOnFail.DurationValue.Rate = DurationRate.Rounds;
                    buffOnFail.DurationValue.DiceType = DiceType.D3;
                    buffOnFail.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    buffOnFail.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
