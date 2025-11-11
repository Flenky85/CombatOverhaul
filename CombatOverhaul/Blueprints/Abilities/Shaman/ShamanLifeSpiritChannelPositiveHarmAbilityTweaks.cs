using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanLifeSpiritChannelPositiveHarmAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanLifeSpiritChannelPositiveHarm)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .SetDescriptionValue(
                    "Channeling positive energy causes a burst that damages all undead creatures in a 30-foot " +
                    "radius centered on the shaman. The amount of damage inflicted is equal to 1d6 points of " +
                    "damage plus 1d6 points of damage for every two shaman levels beyond 1st (2d6 at 3rd, 3d6 " +
                    "at 5th, and so on). Creatures that take damage from channeled energy receive a Will save " +
                    "to halve the damage. The DC of this save is equal to 10 + 1/2 the shaman's level + the " +
                    "shaman's Charisma modifier.\n" +
                    "Activating this ability expends 6 charges. The shaman has a number of charges equal to " +
                    "6 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
