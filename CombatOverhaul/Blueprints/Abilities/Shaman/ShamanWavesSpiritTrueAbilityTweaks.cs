using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
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
    internal static class ShamanWavesSpiritTrueAbilityTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesGuids.ShamanWavesSpiritTrueAbility;
            var icon = BlueprintTool
                .Get<Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbility>("96d2ab91f2d2329459a8dab496c5bede")
                .Icon;

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
                    LocalizationUtils.MakeName(guid, "Elemental Body IV (Water)"))
                .SetDescription(
                    LocalizationUtils.MakeDescription(guid,
                        "You become a huge water elemental.You gain a + 4 size bonus to your Strength, a –2 penalty to your Dexterity, " +
                        "a + 8 size bonus to your Constitution, a + 6 form natural armor bonus to AC, resist cold 20, and vulnerability " +
                        "to fire.You also gain two 2d6 slam attacks and the freeze ability.You are immune to critical hits and sneak " +
                        "attacks while in elemental form and gain DR 5 /—. Your movement speed is reduced by 10 feet.\n" +
                        "Freeze: A water elemental deals 2d6 cold damage in addition to damage dealt on a successful hit in melee. " +
                        "Those affected by the freeze ability must also succeed at a Reflex save or start freezing, taking 2d6 damage " +
                        "each round for an additional 1d4 rounds.Creatures that hit a freezing creature with natural weapons or unarmed " +
                        "attacks take cold damage as though hit by the freezing creature and must succeed at a Reflex save to avoid freezing.\n" +
                        "After using this ability, the shaman must wait 6 rounds before she can use it again."
                    ))
                .SetIcon(icon)
                .Configure();
        }
    }
}
