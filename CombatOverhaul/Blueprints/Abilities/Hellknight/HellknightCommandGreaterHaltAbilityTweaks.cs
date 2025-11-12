using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Hellknight
{
    [AutoRegister]
    internal static class HellknightCommandGreaterHaltAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HellknightCommandGreaterHalt)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)cond.Failed.Actions[0];

                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D3;
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
                    apply.DurationValue.m_IsExtendable = false;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
