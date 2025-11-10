using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexBoneLockAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexBoneLockAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var onSave = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var condA = (Conditional)onSave.Failed.Actions[0];     
                    var condB = (Conditional)condA.IfFalse.Actions[0];     

                    var applyA = (ContextActionApplyBuff)condA.IfTrue.Actions[0];
                    applyA.DurationValue.Rate = DurationRate.Rounds;
                    applyA.DurationValue.DiceType = DiceType.D3;
                    applyA.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    applyA.DurationValue.BonusValue = ContextValues.Constant(0);

                    var applyBTrue = (ContextActionApplyBuff)condB.IfTrue.Actions[0];
                    applyBTrue.DurationValue.Rate = DurationRate.Rounds;
                    applyBTrue.DurationValue.DiceType = DiceType.D3;
                    applyBTrue.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    applyBTrue.DurationValue.BonusValue = ContextValues.Constant(0);

                })
                .SetDescriptionValue(
                    "With a quick incantation, the shaman causes a creature within 30 feet to suffer stiffness in the joints " +
                    "and bones, causing the target to be staggered 1 round. A successful Fortitude saving throw negates this " +
                    "effect. At 8th level, the duration is 2d3 rounds, though " +
                    "the target can attempt a save each round to end the effect if its initial saving throw fails. At 16th " +
                    "level, the target can no longer attempt a saving throw each round to end the effect, although it still " +
                    "attempts the initial Fortitude saving throw to negate the effect entirely."
                )
                .Configure();
        }
    }
}
