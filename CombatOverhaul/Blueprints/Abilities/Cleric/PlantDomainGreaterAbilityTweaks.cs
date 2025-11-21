using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class PlantDomainGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PlantDomainGreaterAbility)
                .SetDescriptionValue(
                    "At 6th level, as a free action you can cause wooden thorns to burst from your skin, forming bramble armor. " +
                    "While it’s active, any foe that hits you with a melee weapon that doesn’t have reach takes 1d6 piercing damage " +
                    "+ 1 point per two levels you have in the class that granted this domain."
                )
                .Configure();
        }
    }
}
