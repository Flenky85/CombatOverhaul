using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class VinetrapAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Vinetrap)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply0 = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply0.Permanent = false;
                    apply0.UseDurationSeconds = false;
                    apply0.DurationValue.Rate = DurationRate.Rounds;
                    apply0.DurationValue.DiceType = DiceType.D3;
                    apply0.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    apply0.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var save = (ContextActionSavingThrow)c.Actions.Actions[1];
                    var cond = (ContextActionConditionalSaved)save.Actions.Actions[0];
                    var applyOnFail = (ContextActionApplyBuff)cond.Failed.Actions[0];

                    applyOnFail.Permanent = false;
                    applyOnFail.UseDurationSeconds = false;
                    applyOnFail.DurationValue.Rate = DurationRate.Rounds;
                    applyOnFail.DurationValue.DiceType = DiceType.D3;
                    applyOnFail.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    applyOnFail.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
