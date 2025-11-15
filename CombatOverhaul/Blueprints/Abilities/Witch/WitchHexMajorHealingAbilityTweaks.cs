using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexMajorHealingAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexMajorHealingAbility)
                .SetDescriptionValue(
                    "This hex acts as cure serious wounds, using the witch's caster level. Once a creature " +
                    "has benefited from the major healing hex, it cannot benefit from it again on new combat. " +
                    "At 15th level, this hex acts like cure critical wounds."
                )
                .Configure();
        }
    }
}
