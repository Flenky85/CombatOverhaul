using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class SerenityAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Serenity)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];
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
                .SetDescriptionValue(
                    "You fill the targets' minds with feelings of tranquility. Those attempting to commit violence " +
                    "become stricken with wracking pain and take 4d6 points of damage each round they attempt to " +
                    "harm another creature."
                )
                .Configure();
        }
    }
}
