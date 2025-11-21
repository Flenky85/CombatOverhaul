using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.ActivatableAbilities;

namespace CombatOverhaul.Blueprints.ActivatableAbilities.Cleric
{
    [AutoRegister]
    internal static class DestructionDomainBaseActivateableAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.DestructionDomainBaseActivateableAbility)
                .RemoveComponents(c => c is ActivatableAbilityResourceLogic)
                .Configure();
        }
    }
}
