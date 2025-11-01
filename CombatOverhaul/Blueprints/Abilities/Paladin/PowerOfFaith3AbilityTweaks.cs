using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;                           
using Kingmaker.UnitLogic.Abilities.Components;       
using Kingmaker.UnitLogic.Mechanics;                  
using Kingmaker.UnitLogic.Mechanics.Actions;          

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class PowerOfFaith3AbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PowerOfFaith3)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue.ValueType = ContextValueType.Simple;
                    apply.DurationValue.DiceCountValue.Value = 0;
                    apply.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    apply.DurationValue.BonusValue.Value = 3;
                })
                .Configure();
        }
    }
}
