using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiTrueStrikeAbilityTweaks
    {
        public static void Register()
        {
            var abilites = new[]
            {
                AbilitiesGuids.KiTrueStrike,
                AbilitiesGuids.DrunkenKiTrueStrike,
                AbilitiesGuids.ScaledFistTrueStrike,
            };
            foreach (var id in abilites)
            {
                AbilityConfigurator.For(id)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "A monk with this ki power can spend 3 point from his ki pool as a standard action to gain " +
                    "temporary intuitive insight into the immediate future during his next attack. The monk's next " +
                    "single attack roll (if it is made before the end of the next round) gains a +20 insight bonus. " +
                    "Additionally, he is not affected by the miss chance that applies to attackers trying to strike a " +
                    "concealed target."
                )
                .Configure();
            }
        }
    }
}
