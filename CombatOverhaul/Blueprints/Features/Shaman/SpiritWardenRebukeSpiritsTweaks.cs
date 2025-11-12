using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Shaman
{
    [AutoRegister]
    internal class SpiritWardenRebukeSpiritsTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.SpiritWardenRebukeSpirits)
                .SetDescriptionValue(
                    "At 2nd level, the spirit warden gains the ability to channel positive energy as a cleric of her level. " +
                    "Regardless of her alignment, she can only use this ability to harm undead creatures.\n" +
                    "Activating this ability expends 6 charges. The shaman has a number of charges equal to " +
                    "6 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
