using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;


namespace CombatOverhaul.Blueprints.AbilityAreaEffect.Spells.Level3
{
    [AutoRegister]
    internal static class StinkingCloudAreaTweaks
    {
        public static void Register()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectGuids.StinkingCloudArea)
                .EditComponent<AbilityAreaEffectRunAction>(c =>
                {
                    var rootExit = (Conditional)c.UnitExit.Actions[0];
                    var applyA = (ContextActionApplyBuff)rootExit.IfTrue.Actions[1];
                    applyA.DurationValue.Rate = DurationRate.Rounds;
                    applyA.DurationValue.DiceType = DiceType.D2;
                    applyA.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };
                    applyA.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var nested = (Conditional)rootExit.IfFalse.Actions[0];
                    var applyB = (ContextActionApplyBuff)nested.IfTrue.Actions[1];
                    applyB.DurationValue.Rate = DurationRate.Rounds;
                    applyB.DurationValue.DiceType = DiceType.D2;
                    applyB.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 };
                    applyB.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .Configure();
        }
    }
}
