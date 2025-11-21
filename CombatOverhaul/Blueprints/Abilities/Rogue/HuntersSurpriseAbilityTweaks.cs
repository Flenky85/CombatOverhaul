using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Rogue
{
    [AutoRegister]
    internal static class HuntersSurpriseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HuntersSurpriseAbility)
                .SetDescriptionValue(
                    "Once per round, a rogue with this talent can designate a single enemy she is adjacent to as her prey. " +
                    "Until the end of her next turn, she can add her sneak attack damage to all attacks made against her prey, " +
                    "even if she is not flanking it or it is not flat-footed."
                )
                .Configure();
        }
    }
}
