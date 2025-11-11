using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.utils;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using UnityEngine;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanStonesSpiritTrueAbilityTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesGuids.ShamanStonesSpiritTrueAbility;
            var icon = BlueprintTool
                .Get<Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbility>("facdc8851a0b3f44a8bed50f0199b83c")
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
                    LocalizationUtils.MakeName(guid, "Elemental Body IV (Earth)"))
                .SetDescription(
                    LocalizationUtils.MakeDescription(guid,
                        "You become a huge earth elemental. You gain a +8 size bonus to your Strength, a –2 penalty to your Dexterity, " +
                        "a +4 size bonus to your Constitution, a +6 form natural armor bonus to AC, and a +1 bonus to your attack, " +
                        "damage rolls, and combat maneuvers. You also gain two 2d8 slam attacks, resist acid 20, and vulnerability to " +
                        "electricity. You are immune to critical hits and sneak attacks while in elemental form and gain DR 5/—. " +
                        "Your movement speed is reduced by 10 feet.\n" +
                        "After using this ability, the shaman must wait 6 rounds before she can use it again."
                    ))
                .SetIcon(icon)
                .Configure();
        }
    }
}
