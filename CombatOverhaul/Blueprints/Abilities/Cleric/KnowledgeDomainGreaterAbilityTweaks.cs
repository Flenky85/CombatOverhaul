using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class KnowledgeDomainGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.KnowledgeDomainGreaterAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .SetDescriptionValue(
                    "At 8th level as a swift action you can grant all allies within 30 feet special insights. " +
                    "Once during the next minute, each affected creature can choose to roll twice and take the " +
                    "better result before attempting an attack roll, ability check, skill check, or saving throw.\n" +
                    "The ability have a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
