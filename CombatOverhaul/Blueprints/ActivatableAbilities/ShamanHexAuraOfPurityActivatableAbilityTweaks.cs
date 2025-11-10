using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.ActivatableAbilities;

namespace CombatOverhaul.Blueprints.ActivatableAbilities
{
    [AutoRegister]
    internal static class ShamanHexAuraOfPurityActivatableAbilityTweaks
    {
        public static void Register()
        {
            ActivatableAbilityConfigurator.For(ActivatableAbilitiesGuids.ShamanHexAuraOfPurityActivatableAbility)
                .EditComponent<ActivatableAbilityResourceLogic>(c =>
                {
                    c.SpendType = ActivatableAbilityResourceLogic.ResourceSpendType.NewRound; 
                })
                .SetDescriptionValue(
                    "Diseases, inhaled poisons, and noxious gaseous effects (such as stinking cloud) are negated in a " +
                    "10-foot aura around the shaman. Effects caused by spells whose level is more than half the shaman's " +
                    "class level are unaffected.\n" +
                    "Starts with 2 charges of this ability and gains 1 additional charge every 5 level in the class. " +
                    "While the aura is active, it consumes 1 charge each round.\n" +
                    "While the aura is not active, regains 1 charge at the start of each of her turns.\n"
                )
                .Configure();
        }
    }
}
