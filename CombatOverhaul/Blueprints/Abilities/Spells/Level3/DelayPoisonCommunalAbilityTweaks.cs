using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class DelayPoisonCommunalAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DelayPoisonCommunal)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.Permanent = false;
                    apply.UseDurationSeconds = false;
                    apply.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        },
                        BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 12
                        },
                        m_IsExtendable = true
                    };
                })
                .SetDuration12RoundsShared()
                .Configure();
        }
    }
}
