using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level1
{
    [AutoRegister]
    internal static class AcidMawAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AcidMaw)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var onPet = (ContextActionsOnPet)c.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)onPet.Actions.Actions[0];

                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };
                })
                .EditComponent<AdditionalDiceOnAttack>(ad =>
                {
                    ad.Value.DiceType = DiceType.D6;
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "Your animal companion's bite attack deals an additional 1d6 points of acid damage, " +
                    "and the acid deals another 1d6 points of acid damage to the target on the next round. " +
                    "The acid continues to deal damage for 1 additional round per 2 caster levels (maximum of 3). " +
                    "This ongoing acid damage doesn't stack from multiple attacks, but the duration resets if a " +
                    "newer bite would cause the ongoing damage to last longer than the duration remaining from a " +
                    "previous one. The acid does not harm the animal companion."
                )
                .Configure();
        }
    }
}
