using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class ShadowFormAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShadowFormAbility)
                .SetActionType(UnitCommand.CommandType.Swift)  
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var applyA = (ContextActionApplyBuff)root.IfFalse.Actions[0];
                    applyA.DurationValue.Rate = DurationRate.Rounds;
                    applyA.DurationValue.DiceType = DiceType.Zero;
                    applyA.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyA.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var nested = (Conditional)root.IfFalse.Actions[1];

                    var applyB = (ContextActionApplyBuff)nested.IfTrue.Actions[0];
                    applyB.DurationValue.Rate = DurationRate.Rounds;
                    applyB.DurationValue.DiceType = DiceType.Zero;
                    applyB.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyB.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };

                    var applyC = (ContextActionApplyBuff)nested.IfFalse.Actions[0];
                    applyC.DurationValue.Rate = DurationRate.Rounds;
                    applyC.DurationValue.DiceType = DiceType.Zero;
                    applyC.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    applyC.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "At 20th level, as a swift action, a shadowcaster can assume a form of pure shadow for 6 rounds. While in this form, " +
                    "the shadowcaster is considered incorporeal and undead, gains a +2 bonus to Intelligence, an incorporeal touch attack " +
                    "(2d8 points of Strength damage), immunity to ground-based effects, and a +30-foot bonus to speed. The shadowcaster also " +
                    "gains the effects of the transformation spell while still retaining his ability to cast spells. The shadowcaster can use " +
                    "this ability once per combat."
                )
                .Configure();
        }
    }
}
