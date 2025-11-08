using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class ForcedRepentanceAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ForcedRepentance)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.Actions.Actions[0];
                    var save = (ContextActionConditionalSaved)cond.IfTrue.Actions[0];
                    var apply = (ContextActionApplyBuff)save.Failed.Actions[0];

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
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
