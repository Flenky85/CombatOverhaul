using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.utils;
using CombatOverhaul.Utils;
using Kingmaker.Cheats;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Pathfinding;
using System.Drawing;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanWindSpiritTrueAbilityTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesGuids.ShamanWindSpiritTrueAbility;

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
                        "You become a huge air elemental. You gain a +4 size bonus to your Strength, a +6 size bonus to your Dexterity, " +
                        "a +4 form natural armor bonus to AC, resist electricity 20, and vulnerability to acid. You also gain two 2d6 " +
                        "slam attacks and the whirlwind ability. You are immune to critical hits and sneak attacks while in elemental " +
                        "form and gain DR 5/—. Your movement speed is increased by 30 feet.\n" +
                        "Whirlwind: An air elemental can transform itself into a whirlwind and back again.The whirlwind is 40 feet wide, " +
                        "and every creature that spends a round in the whirlwind must succeed at a Reflex save or take 2d6 bludgeoning " +
                        "damage. While in whirlwind form, the elemental cannot attack, but it is able to use its abilities.\n" +
                        "After using this ability, the shaman must wait 6 rounds before she can use it again."
                    ))
                .Configure();
        }
    }
}
