using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.utils;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanFlameSpiritTrueAbilityTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesGuids.ShamanFlameSpiritTrueAbility;

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
                .SetDisplayName(
                    LocalizationUtils.MakeName(guid, "Elemental Body IV (Fire)"))
                .SetDescription(
                    LocalizationUtils.MakeDescription(guid,
                        "You become a huge fire elemental. You gain a +6 size bonus to your Dexterity, a +4 size bonus to your Constitution, " +
                        "a +4 form natural armor bonus to AC, resist fire 20, and vulnerability to cold. You also gain two 2d6 slam attacks " +
                        "and the burn ability. You are immune to critical hits and sneak attacks while in elemental form and gain DR 5/—. " +
                        "Your movement speed is increased by 20 feet.\n" +
                        "Burn: A fire elemental deals 2d6 fire damage in addition to damage dealt on a successful hit in melee.Those affected " +
                        "by the burn ability must also succeed at a Reflex save or catch fire, taking 2d6 damage each round for an additional " +
                        "1d4 rounds.Creatures that hit a burning creature with natural weapons or unarmed attacks take fire damage as though " +
                        "hit by the burning creature and must succeed at a Reflex save to avoid catching fire.\n" +
                        "After using this ability, the shaman must wait 6 rounds before she can use it again."
                    ))
                .Configure();
        }
    }
}
