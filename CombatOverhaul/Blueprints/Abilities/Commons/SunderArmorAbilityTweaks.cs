using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace CombatOverhaul.Blueprints.Abilities.Commons
{
    [AutoRegister]
    internal static class SunderArmorAbilityTweaks
    {
        public static void Register()
        {
            var id = AbilitiesGuids.SunderArmor;

            var ability = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(id);
            if (ability == null) return;

            const string desc =
                "You can attempt to dislodge a piece of armor worn by your opponent. If your combat maneuver is successful, " +
                "the target loses its bonuses from armor for 1 round.\n" +
                "For every 5 by which your attack exceeds your opponent's CMD, the penalty lasts 1 additional round.\n" +
                "In addition, while this effect lasts, the target’s damage reduction from armor is halved. This reduction also applies to monsters.";

            AbilityConfigurator.For(id)
                .SetDescriptionValue(desc)
                .Configure();
        }
    }
}
