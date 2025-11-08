using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class JoltingPortentAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.JoltingPortent)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];

                    apply.UseDurationSeconds = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;

                    apply.DurationValue.DiceType = DiceType.D4;
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
                .SetDuration2d4RoundsShared()
                .Configure();
        }
    }
}
