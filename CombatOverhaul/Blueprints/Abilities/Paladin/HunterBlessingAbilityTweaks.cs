using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class HunterBlessingAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HunterBlessing)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var a0 = (ContextActionApplyBuff)c.Actions.Actions[0];
                    a0.Permanent = false;
                    a0.UseDurationSeconds = false;
                    a0.SameDuration = false;
                    a0.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 4 },
                        m_IsExtendable = true
                    };
                })
                .Configure();
        }
    }
}
