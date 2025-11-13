using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class ElementalFistAblilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ElementalFistAblility)
                .SetDescriptionValue(
                    "Benefit: When you use Elemental Fist, pick one of the following energy types: acid, cold, electricity, " +
                    "or fire. On a successful hit, the attack deals damage normally plus 1d6 points of damage of the chosen " +
                    "type. You must declare that you are using this feat before you make your attack roll " +
                    "(thus a failed attack roll ruins the attempt).\n" +
                    "This ability consumes 2 charges per activation; you begin with 1 charge, gain 1 additional charge " +
                    "every 4 monk levels, and regenerate 1 charge each round."
                )
                .Configure();
        }
    }
}
