using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexAirBarrierAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexAirBarrierAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var applyBuff = (ContextActionApplyBuff)c.Actions.Actions[0];

                    applyBuff.DurationValue.Rate = DurationRate.Rounds;
                    applyBuff.DurationValue.BonusValue.Value = 3;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3;
                })
                .SetDescriptionValue(
                    "The shaman creates an invisible shell of air that grants her a +4 armor bonus to AC. At " +
                    "7th level and every 4 levels thereafter, this bonus increases by 2. At 13th level, this " +
                    "barrier causes incoming arrows, rays, and other ranged attacks requiring an attack roll " +
                    "against her to suffer a 50% miss chance.\n" +
                    "This ability uses a pool of charges.Activating the barrier expends 3 charges and grants the " +
                    "shaman a buff that lasts for 3 rounds.The shaman begins with 3 charges; at 10th level and " +
                    "20th level she gains 3 additional charges each time(for a total of 6 and 9 charges, " +
                    "respectively).At the start of each of her turns, she regains 1 expended charge, but only " +
                    "while the buff is not active."
                )
                .Configure();
        }
    }
}
