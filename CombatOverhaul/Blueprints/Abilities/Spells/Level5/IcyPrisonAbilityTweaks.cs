using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class IcyPrisonAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.IcyPrison)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];

                    var applySucceed = (ContextActionApplyBuff)saved.Succeed.Actions[0];
                    applySucceed.UseDurationSeconds = false;
                    applySucceed.DurationValue.Rate = DurationRate.Rounds;
                    applySucceed.DurationValue.DiceType = DiceType.D3;
                    applySucceed.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    applySucceed.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var applyFailed = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    applyFailed.UseDurationSeconds = false;
                    applyFailed.DurationValue.Rate = DurationRate.Rounds;
                    applyFailed.DurationValue.DiceType = DiceType.D3;
                    applyFailed.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    applyFailed.DurationValue.BonusValue = new ContextValue
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
