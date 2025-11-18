using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Progression.Wizard
{
    [AutoRegister]
    internal class SpecialisationSchoolIllusionProgressionTweaks
    {
        public static void Register()
        {
            ProgressionConfigurator.For(ProgressionGuids.SpecialisationSchoolIllusionProgression)
                .SetDescriptionValue(
                    "Illusionists use magic to weave confounding images, figments, and phantoms to baffle and vex their foes.\n" +
                    "Blinding Ray: As a standard action, you can fire a shimmering ray at any foe within 30 feet as a ranged touch attack. " +
                    "The ray causes creatures to be blinded for 1 round. Creatures with more Hit Dice than your wizard level " +
                    "are dazzled for 1 round instead.\n" +
                    "Invisibility Field : At 8th level, you can make yourself invisible as a swift action. " +
                    "This otherwise functions as invisibility, greater." +
                    "Invisivility field starts with 3 charges and gains 1 additional charge every 5 levels; you can spend 1 charge for each round " +
                    "the ability is active, and you regain 1 charge each round while the ability is not active."
                )
                .Configure();
        }
    }
}
