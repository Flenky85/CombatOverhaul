using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;


namespace CombatOverhaul.Blueprints.AbilityAreaEffect.Spells.Level6
{
    [AutoRegister]
    internal static class CloakofDreamsAreaAbilityTweaks
    {
        public static void Register()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectGuids.CloakofDreamsArea)
                .EditComponent<AbilityAreaEffectRunAction>(c =>
                {
                    var cond1 = (Conditional)c.Round.Actions[0];
                    var cond2 = (Conditional)cond1.IfFalse.Actions[0];
                    var saving = (ContextActionSavingThrow)cond2.IfFalse.Actions[0];
                    var saved = (ContextActionConditionalSaved)saving.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)saved.Failed.Actions[0];

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
                    apply.DurationValue.m_IsExtendable = false;
                })
                .Configure();
        }
    }
}
