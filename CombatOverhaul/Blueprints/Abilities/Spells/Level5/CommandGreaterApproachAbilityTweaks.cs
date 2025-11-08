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
    internal static class CommandGreaterApproachAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CommandGreaterApproach)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var onSave = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)onSave.Failed.Actions[0];

                    apply.UseDurationSeconds = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D3;
                    apply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    apply.DurationValue.m_IsExtendable = true;
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
