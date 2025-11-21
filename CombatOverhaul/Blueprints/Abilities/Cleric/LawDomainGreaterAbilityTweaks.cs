using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class LawDomainGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.LawDomainGreaterAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var enchant = (ContextActionEnchantWornItem)c.Actions.Actions[1];
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
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "At 8th level, you can give a weapon you touch the axiomatic special weapon quality for a number of rounds 6 " +
                    "rounds." +
                    "The ability have a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
