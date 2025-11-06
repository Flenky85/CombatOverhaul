using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class ShoutGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShoutGreater)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var condSaved = (ContextActionConditionalSaved)c.Actions.Actions[2];
                    var apply = (ContextActionApplyBuff)condSaved.Failed.Actions[0];

                    apply.UseDurationSeconds = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D6;
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
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "You create a bolt of dark energy and use it to make a ranged touch attack that ignores concealment " +
                    "(but not total concealment).\n" +
                    "If you hit, the target takes 1d6 points of damage per caster level(maximum 14d6).Half of this damage " +
                    "is cold damage and half of it is negative energy. The bolt's shadow expands and covers the target, " +
                    "rendering them blind for the duration of the spell. A successful Fortitude save halves the damage " +
                    "and negates the blind condition."
                )
                .Configure();
        }
    }
}
