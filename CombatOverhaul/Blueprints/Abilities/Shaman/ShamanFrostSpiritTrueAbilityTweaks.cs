using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.utils;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanFrostSpiritTrueAbilityTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesGuids.ShamanFrostSpiritTrueAbility;

            AbilityConfigurator.For(guid)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.Permanent = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = ContextValues.Constant(0);
                    apply.DurationValue.BonusValue = ContextValues.Constant(3);
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .SetDuration3RoundsShared()
                .SetDescription(
                    LocalizationUtils.MakeDescription(guid,
                        "You become a large smilodon. You gain a +4 size bonus to your Strength, a –2 penalty to your Dexterity, " +
                        "and a +4 form natural armor bonus to AC. Your movement speed is increased by 10 feet. You also gain two 1d6 " +
                        "claw attacks, one 1d8 bite attack, and the pounce ability.\n" +
                        "Pounce: When a smilodon makes a charge, it can make a full attack.\n" +
                        "After using this ability, the shaman must wait 6 rounds before she can use it again."
                    ))
                .Configure();
        }
    }
}
