using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class CallLightningAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CallLightning)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[1];
                    apply.UseDurationSeconds = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 12 };
                })
                .SetDuration12RoundsShared()
                .SetDescriptionValue(
                    "Immediately upon completion of the spell, and once per round thereafter, you may call down a 6-foot-wide, " +
                    "30-foot-long, vertical bolt of lightning that deals 3d6 points of electricity damage. The bolt of lightning " +
                    "flashes down in a vertical stroke at whatever target point you choose within the spell's range (measured from " +
                    "your position at the time). Any creature in the target point or in the path of the bolt is affected. You need " +
                    "not call a bolt of lightning immediately; other actions, even spellcasting, can be performed first. Each round " +
                    "after the first you may use a swift action (concentrating on the spell) to call a bolt. Each time you cast a " +
                    "bolt, the spell's duration is reduced by one round."
                )
                .Configure();
        }
    }
}
