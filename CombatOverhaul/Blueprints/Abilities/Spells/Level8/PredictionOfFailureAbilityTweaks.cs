using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class PredictionOfFailureAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PredictionOfFailure)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];

                    var succ1 = (ContextActionApplyBuff)saved.Succeed.Actions[0];
                    succ1.UseDurationSeconds = false;
                    succ1.DurationValue.Rate = DurationRate.Rounds;
                    succ1.DurationValue.DiceType = DiceType.D3;
                    succ1.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    succ1.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var succ2 = (ContextActionApplyBuff)saved.Succeed.Actions[1];
                    succ2.UseDurationSeconds = false;
                    succ2.DurationValue.Rate = DurationRate.Rounds;
                    succ2.DurationValue.DiceType = DiceType.D3;
                    succ2.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    succ2.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDurationPermanet2d3RoundsShared()
                .SetDescriptionValue(
                    "You wrack the target's body and mind with the anguish and suffering of every bitter failure " +
                    "it will ever experience, rendering it permanently shaken and sickened. A successful Will save " +
                    "reduces the duration to 2d3 rounds."
                )
                .Configure();
        }
    }
}
