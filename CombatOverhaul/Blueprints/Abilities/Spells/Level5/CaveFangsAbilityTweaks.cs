using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class CaveFangsAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CaveFangs)
                .SetDuration12RoundsShared()
                .SetDescriptionValue(
                    "You create a magical trap in the area that causes deadly stalactites or stalagmites to (bite) an intruder. " +
                    "The magical trap is triggered whenever an enemy creature moves through the affected area. The effect of cave " +
                    "fangs depends on whether you create stalactites or stalagmites (see below). You can place these traps anywhere " +
                    "within close range as swift action for the duration of the spell; the trap's radius is 5 feet. Each time you place a trap, " +
                    "the spell's duration is reduced by 1 round.\n" +
                    "Stalactites: Shards of rock drop from above, dealing 3d8 points of bludgeoning and piercing damage(Reflex half).A " +
                    "creature that fails its Reflex save is pinned to the ground under stalactites and rubble, gaining the entangled " +
                    "condition until it can free itself with a successful DC 15 Strength check or DC 20 Mobility check.\n" +
                    "Stalagmites: Piercing spires of rock erupt up from the ground, dealing 3d8 points of piercing damage and knocking " +
                    "the creature prone(a successful Reflex saving throw halves this damage and avoids being knocked prone).Once the " +
                    "stalagmites appear, they function thereafter as spike stones for 2d3 rounds and then crumble to dust."
                )
                .Configure();
        }
    }
}
