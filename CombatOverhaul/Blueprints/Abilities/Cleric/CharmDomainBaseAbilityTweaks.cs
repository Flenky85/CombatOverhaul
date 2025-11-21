using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class CharmDomainBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CharmDomainBaseAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "You can cause a living creature to become dazed for 1 round as a melee touch attack. " +
                    "Creatures with more Hit Dice than your level in the class that gave you access to this " +
                    "domain are unaffected."
                )
                .Configure();
        }
    }
}
