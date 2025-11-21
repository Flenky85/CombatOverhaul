using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.ActivatableAbilities;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Cleric
{
    [AutoRegister]
    internal static class LiberationDomainBaseActivateableAbility
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.LiberationDomainBaseActivateableAbility)
                .RemoveComponents(c => c is ActivatableAbilityResourceLogic)
                .SetDescriptionValue(
                    "At 8th level, you can emit a 30-foot aura of freedom for a number of rounds per day equal to your level in " +
                    "the class that gave you access to this domain. Allies within this aura are not affected by difficult terrain " +
                    "or the confused, frightened, paralyzed, slowed, shaken, or staggered conditions.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the aura is active, it consumes 1 charge each round.\n" +
                    "While the aura is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
