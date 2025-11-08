using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class DisruptingWeaponAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DisruptingWeapon)
                .SetActionType(UnitCommand.CommandType.Free)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var enchant = (ContextActionEnchantWornItem)c.Actions.Actions[0];
                    enchant.DurationValue.Rate = DurationRate.Rounds;
                    enchant.DurationValue.DiceType = DiceType.Zero;
                    enchant.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    enchant.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };

                    var apply = (ContextActionApplyBuff)c.Actions.Actions[1];
                    apply.Permanent = false;
                    apply.UseDurationSeconds = false;
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
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
