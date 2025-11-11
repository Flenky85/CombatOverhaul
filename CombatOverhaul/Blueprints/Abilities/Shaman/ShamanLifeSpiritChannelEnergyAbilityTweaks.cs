using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanLifeSpiritChannelEnergyAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanLifeSpiritChannelEnergy)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 6;
                })
                .SetDescriptionValue(
                    "Channeling positive energy causes a burst that heals all living creatures in a 30-foot radius " +
                    "centered on the shaman. The amount of damage healed is equal to 1d6 points of damage " +
                    "plus 1d6 points of damage for every two shaman levels beyond 1st (2d6 at 3rd, 3d6 at 5th, and so on).\n" +
                    "Activating this ability expends 6 charges. The shaman has a number of charges equal to " +
                    "6 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
