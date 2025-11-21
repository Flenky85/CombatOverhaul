using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Cleric
{
    [AutoRegister]
    internal static class ProtectionDomainGreaterToggleAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.ProtectionDomainGreaterToggleAbility)
                .SetDescriptionValue(
                    "At 8th level, you can emit a 30-foot aura of protection. You and your allies within this aura gain a +1 " +
                    "deflection bonus to AC and resistance 5 against all elements (acid, cold, electricity, fire, and sonic). " +
                    "The deflection bonus increases by +1 for every four levels you possess in the class that gave you access " +
                    "to this domain beyond 8th. At 14th level, the resistance against all elements increases to 10.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the aura is active, it consumes 1 charge each round.\n" +
                    "While the aura is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
