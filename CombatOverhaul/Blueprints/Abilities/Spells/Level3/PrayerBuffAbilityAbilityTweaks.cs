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
    internal static class PrayerBuffAbilityAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PrayerBuffAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var applySelf = (ContextActionApplyBuff)c.Actions.Actions[0];
                    applySelf.Permanent = false;
                    applySelf.UseDurationSeconds = false;
                    applySelf.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 },
                        m_IsExtendable = true
                    };

                    var party = (ContextActionPartyMembers)c.Actions.Actions[1];
                    var applyParty = (ContextActionApplyBuff)party.Action.Actions[0];
                    applyParty.Permanent = false;
                    applyParty.UseDurationSeconds = false;
                    applyParty.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 },
                        m_IsExtendable = true
                    };
                })
                .Configure();
        }
    }
}
