using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class WavesOfEctasyAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WavesOfEctasy)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var applyStaggered = (ContextActionApplyBuff)saved.Failed.Actions[1];
                    applyStaggered.UseDurationSeconds = false;
                    applyStaggered.DurationValue.Rate = DurationRate.Rounds;
                    applyStaggered.DurationValue.DiceType = DiceType.D3;
                    applyStaggered.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2 
                    };
                    applyStaggered.DurationValue.BonusValue = new ContextValue
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
