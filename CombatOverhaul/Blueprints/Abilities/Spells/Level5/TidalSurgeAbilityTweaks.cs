using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class TidalSurgeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TidalSurge)
                .SetDescriptionValue(
                    "You create an onrushing surge of water 10 feet high in either a 30-foot cone or a 60-foot line that deals " +
                    "1d8 points of bludgeoning damage per caster level (maximum 12d8). In addition to taking damage, creatures " +
                    "that fail their Reflex saves are pushed 1d4×5 feet away from you and are knocked prone. Magical effects with " +
                    "fire descriptor on targets in the area of a tidal surge and similar area effects in the selected point are " +
                    "affected as if you had cast dispel magic."
                )
                .Configure();
        }
    }
}
