using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class MagicDomainBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MagicDomainBaseAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "You can cause your melee weapon to fly from your grasp and strike a foe before instantly returning. " +
                    "As a standard action, you can make a single attack using a melee weapon at a range of 30 feet. This " +
                    "attack is treated as a ranged attack with a thrown weapon. This ability " +
                    "cannot be used to perform a combat maneuver."
                )
                .Configure();
        }
    }
}
