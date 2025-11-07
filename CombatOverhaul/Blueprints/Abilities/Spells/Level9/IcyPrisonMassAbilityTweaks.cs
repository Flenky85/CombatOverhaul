using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class IcyPrisonMassAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.IcyPrisonMass)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];

                    var applyOnSuccess = (ContextActionApplyBuff)saved.Succeed.Actions[0];
                    applyOnSuccess.UseDurationSeconds = false;
                    applyOnSuccess.DurationValue.Rate = DurationRate.Rounds;
                    applyOnSuccess.DurationValue.DiceType = DiceType.D3;
                    applyOnSuccess.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2 
                    };
                    applyOnSuccess.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var applyOnFail = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    applyOnFail.UseDurationSeconds = false;
                    applyOnFail.DurationValue.Rate = DurationRate.Rounds;
                    applyOnFail.DurationValue.DiceType = DiceType.D3;
                    applyOnFail.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    applyOnFail.DurationValue.BonusValue = new ContextValue
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
