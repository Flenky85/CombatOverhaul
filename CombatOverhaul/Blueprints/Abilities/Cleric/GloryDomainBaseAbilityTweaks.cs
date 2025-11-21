using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class GloryDomainBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.GloryDomainBaseAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "You can cause your hand to shimmer with divine radiance, allowing you to touch a creature as a " +
                    "standard action and give it a bonus equal to your level in the class that gave you access to this " +
                    "domain on a single Charisma-based skill check or Charisma ability check. This ability lasts for 1 " +
                    "hour or until the creature touched applies the bonus to a roll."
                )
                .Configure();
        }
    }
}
