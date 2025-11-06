using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;


namespace CombatOverhaul.Blueprints.AbilityAreaEffect.Spells.Level6
{
    [AutoRegister]
    internal static class SiroccoAreaAbilityTweaks
    {
        public static void Register()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectGuids.SiroccoArea)
                .EditComponent<AbilityAreaEffectRunAction>(c =>
                {
                    var save = (ContextActionSavingThrow)c.Round.Actions[0];
                    var cond = (Conditional)save.Actions.Actions[1];
                    var dmgTrue = (ContextActionDealDamage)cond.IfTrue.Actions[0];
                    dmgTrue.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    
                    var dmgFalse = (ContextActionDealDamage)cond.IfFalse.Actions[0];
                    dmgFalse.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .Configure();
        }
    }
}
