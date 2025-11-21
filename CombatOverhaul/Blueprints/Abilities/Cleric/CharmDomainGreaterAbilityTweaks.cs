using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class CharmDomainGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CharmDomainGreaterAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3;
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "At 8th level, you can cast charm person as a swift action, with a DC of 10 + 1/2 your level in the class " +
                    "that gave you access to this domain + your Wisdom modifier. Effect of charm person lasts for 1 round.\n" +
                    "The ability have a cooldown of 3 rounds."
                )
                .Configure();
        }
    }
}
