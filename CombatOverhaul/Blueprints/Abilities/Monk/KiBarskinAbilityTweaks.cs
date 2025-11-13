using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiBarskinAbilityTweaks
    {
        public static void Register()
        {
            var abilites = new[]
            {
                AbilitiesGuids.KiBarskin,
                AbilitiesGuids.DrunkenKiBarskin,
                AbilitiesGuids.ScaledFistBarkskin,
            };
            foreach (var id in abilites)
            {
                AbilityConfigurator.For(id)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                    apply.DurationValue.m_IsExtendable = false;
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "A monk with this ki power can spend 3 point from his ki pool as a standard " +
                    "action to grant himself a tough skin. The effect grants a +2 enhancement bonus " +
                    "to the monk's existing natural armor bonus to AC. This enhancement bonus increases " +
                    "by 1 for every three caster levels above 3rd, to a maximum of +5 at 12th level."
                )
                .Configure();
            }
        }
    }
}
