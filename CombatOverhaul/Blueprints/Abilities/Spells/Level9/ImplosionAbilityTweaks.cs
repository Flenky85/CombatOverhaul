using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class ImplosionAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Implosion)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply1 = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply1.UseDurationSeconds = false;
                    apply1.DurationValue.Rate = DurationRate.Rounds;
                    apply1.DurationValue.DiceType = DiceType.Zero;
                    apply1.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    apply1.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 3
                    };

                    var apply2 = (ContextActionApplyBuff)c.Actions.Actions[1];
                    apply2.UseDurationSeconds = false;
                    apply2.DurationValue.Rate = DurationRate.Rounds;
                    apply2.DurationValue.DiceType = DiceType.Zero;
                    apply2.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    apply2.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 3
                    };
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "Each round, including immediately upon casting the spell, you can cause one creature to collapse in " +
                    "on itself, inflicting 1d6 points of damage per caster level (20d6 maximun) (Fortitude saving throw negates). " +
                    "Choosing a new target for this spell is a move action. You can target a particular creature only once with each " +
                    "casting of the spell. Implosion has no effect on creatures with no material body or on incorporeal creatures."
                )
                .Configure();
        }
    }
}
