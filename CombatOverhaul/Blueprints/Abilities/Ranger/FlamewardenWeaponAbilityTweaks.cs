using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Ranger
{
    [AutoRegister]
    internal static class FlamewardenWeaponAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FlamewardenWeaponAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply1 = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply1.DurationValue.Rate = DurationRate.Rounds;
                    apply1.DurationValue.DiceType = DiceType.Zero;
                    apply1.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply1.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };

                    var cond = (Conditional)c.Actions.Actions[1];
                    var apply2 = (ContextActionApplyBuff)cond.IfTrue.Actions[0];
                    apply2.DurationValue.Rate = DurationRate.Rounds;
                    apply2.DurationValue.DiceType = DiceType.Zero;
                    apply2.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply2.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3; 
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "At 4th level, a flamewarden can cause his weapons to burst into flame. As a swift action, the ranger can grant " +
                    "a single weapon he holds the flaming special ability for 3 rounds; while under this effect, the weapon counts as " +
                    "magic for the purpose of overcoming damage reduction.\n" +
                    "Touch of flame uses charges; activating this ability expends 3 charges. The ranger begins with 3 " +
                    "charges + his Wisdom modifier. " +
                    "At the start of each round, the ranger regains 1 charge, up to her maximum number of charges. While touch " +
                    "of flame is active, you do not regain charges at the start of each round."
                )
                .Configure();
        }
    }
}
