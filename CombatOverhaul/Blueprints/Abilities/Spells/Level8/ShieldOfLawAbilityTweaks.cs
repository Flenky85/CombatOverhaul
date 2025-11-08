using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class ShieldOfLawAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShieldOfLaw)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.UseDurationSeconds = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };
                })
                .SetDuration6RoundsShared()
                .Configure();

            
        }
    }
}
