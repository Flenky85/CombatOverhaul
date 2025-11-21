using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Ranger
{
    [AutoRegister]
    internal static class MasterHunterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MasterHunterAbility)
                .SetDescriptionValue(
                    "A ranger of 20th level becomes a master hunter. He can, as a standard action, " +
                    "make a single attack against a favored enemy at his full attack bonus. If the " +
                    "attack hits, the target takes damage normally and must make a successful Fortitude " +
                    "save or die. The DC of this save is equal to 10 + 1/2 the ranger's level + the " +
                    "ranger's Wisdom modifier. A ranger can use this ability once per combat against " +
                    "each of his favorite enemy types."
                )
                .Configure();
        }
    }
}
