using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class PrayerDebuffAbilityAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PrayerDebuffAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var applyEnemy = (ContextActionApplyBuff)c.Actions.Actions[0];
                    applyEnemy.Permanent = false;
                    applyEnemy.UseDurationSeconds = false;
                    applyEnemy.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.D3,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        m_IsExtendable = true
                    };
                })
                .Configure();
        }
    }
}
