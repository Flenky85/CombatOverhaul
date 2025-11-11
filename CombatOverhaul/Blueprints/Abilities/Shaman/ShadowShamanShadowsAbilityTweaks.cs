using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShadowShamanShadowsAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShadowShamanShadowsAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var inner = (Conditional)root.IfTrue.Actions[0];
                    var applyA = (ContextActionApplyBuff)inner.IfTrue.Actions[0];
                    applyA.DurationValue.Rate = DurationRate.Rounds;
                    applyA.DurationValue.DiceType = DiceType.Zero;
                    applyA.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    applyA.DurationValue.BonusValue = ContextValues.Constant(3);

                    var applyB = (ContextActionApplyBuff)root.IfTrue.Actions[1];
                    applyB.DurationValue.Rate = DurationRate.Rounds;
                    applyB.DurationValue.DiceType = DiceType.Zero;
                    applyB.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    applyB.DurationValue.BonusValue = ContextValues.Constant(3);

                    var applyC = (ContextActionApplyBuff)root.IfFalse.Actions[0];
                    applyC.DurationValue.Rate = DurationRate.Rounds;
                    applyC.DurationValue.DiceType = DiceType.Zero;
                    applyC.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    applyC.DurationValue.BonusValue = ContextValues.Constant(3);
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "At 4th level, a shadow shaman gains the ability to use the shadows around to conceal himself. " +
                    "A number of times per day equal to his Charisma modifier, a shadow shaman can gain the benefits " +
                    "of the blur spell for 1 minute.\n" +
                    "Starting at 9th level, he gains the benefits of displacement instead.\n" +
                    "Starting at 14th level, he additionally receives the effect of invisibility, greater.\n" +
                    "This ability has a 6 round cooldown."
                )
                .Configure();
        }
    }
}
