using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.AbilityAreaEffect.Cleric
{
    [AutoRegister]
    internal static class RuneDomainGreaterAreaTweaks
    {
        public static void Register()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectGuids.RuneDomainGreaterArea)
                .EditComponent<AbilityAreaEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.UnitEnter.Actions[0];
                    var save = (ContextActionSavingThrow)cond.IfTrue.Actions[0];
                    var saved = (ContextActionConditionalSaved)save.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)saved.Failed.Actions[0];

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
                .Configure();
        }
    }
}
