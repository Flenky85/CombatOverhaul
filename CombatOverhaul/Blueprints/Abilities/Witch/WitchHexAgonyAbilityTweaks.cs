using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexAgonyAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexAgonyAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (ContextActionConditionalSaved)c.Actions.Actions[1];
                    var apply = (ContextActionApplyBuff)cond.Failed.Actions[0];

                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D3;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "With a quick incantation, a witch can place this hex on one creature within 60 feet, causing them to suffer intense pain. " +
                    "The target is nauseated for 2d3 rounds. A Fortitude save negates this effect. If the saving throw is failed, the target " +
                    "can attempt a new save each round to end the effect. Whether or not the save is successful, a creature cannot be the " +
                    "target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
