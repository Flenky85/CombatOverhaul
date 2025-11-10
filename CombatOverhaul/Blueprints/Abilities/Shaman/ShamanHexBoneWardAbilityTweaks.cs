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
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexBoneWardAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexBoneWardAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply0 = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply0.Permanent = false;
                    apply0.DurationValue.Rate = DurationRate.Rounds;
                    apply0.DurationValue.DiceType = DiceType.Zero;
                    apply0.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    apply0.DurationValue.BonusValue = ContextValues.Constant(6);

                    var condCaster = (Conditional)c.Actions.Actions[1];
                    var apply1 = (ContextActionApplyBuff)condCaster.IfTrue.Actions[0];
                    apply1.DurationValue.Rate = DurationRate.Rounds;
                    apply1.DurationValue.DiceType = DiceType.Zero;
                    apply1.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    apply1.DurationValue.BonusValue = ContextValues.Constant(6);

                    var condSV = (Conditional)c.Actions.Actions[2];
                    var apply2 = (ContextActionApplyBuff)condSV.IfTrue.Actions[0];
                    apply2.DurationValue.Rate = DurationRate.Rounds;
                    apply2.DurationValue.DiceType = DiceType.Zero;
                    apply2.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    apply2.DurationValue.BonusValue = ContextValues.Constant(6);

                    var inner = (Conditional)condSV.IfFalse.Actions[0];
                    var apply3 = (ContextActionApplyBuff)inner.IfTrue.Actions[0];
                    apply3.DurationValue.Rate = DurationRate.Rounds;
                    apply3.DurationValue.DiceType = DiceType.Zero;
                    apply3.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    apply3.DurationValue.BonusValue = ContextValues.Constant(6);

                    var apply4 = (ContextActionApplyBuff)inner.IfFalse.Actions[0];
                    apply4.DurationValue.Rate = DurationRate.Rounds;
                    apply4.DurationValue.DiceType = DiceType.Zero;
                    apply4.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    apply4.DurationValue.BonusValue = ContextValues.Constant(6);
                })
                .SetDescriptionValue(
                    "A shaman touches a willing creature (including herself) and grants a bone ward. The warded " +
                    "creature becomes encircled by a group of flying bones that grant it a +2 deflection bonus to " +
                    "AC for 6 rounds. At 8th level, the ward increases to +3. " +
                    "At 16th level, the bonus increases to +4. A creature " +
                    "affected by this hex cannot be affected by it again on new combat."
                )
                .Configure();
        }
    }
}
