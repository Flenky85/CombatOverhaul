using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class DivineGuardianTrothTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DivineGuardianTroth)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)cond.IfTrue.Actions[0];

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
