using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Progression.Wizard
{
    [AutoRegister]
    internal class SpecialisationSchoolUniversalistProgressionTweaks
    {
        public static void Register()
        {
            ProgressionConfigurator.For(ProgressionGuids.SpecialisationSchoolUniversalistProgression)
                .SetDescriptionValue(
                    "Wizards who do not specialize (known as universalists) have the most diversity of all arcane spellcasters.\n" +
                    "Hand of the Apprentice: You cause your melee weapon to fly from your grasp and strike a foe before instantly returning to you. " +
                    "As a standard action, you can make a single attack using a melee weapon at a range of 30 feet. This attack is treated as a ranged attack " +
                    "with a thrown weapon. This ability cannot be used to perform a combat maneuver.\n" +
                    "Metamagic Mastery: At 8th level, you can extend or alter the range of your next spell as though using the Extend Spell or Reach Spell feat accordingly, " +
                    "Extend and reach costs 2 charges of metamagic mastery.\n" +
                    "At 12th level, you can empower your next spell as though using the Empower Spell feat. Empowering costs 4 charges of metamagic mastery.\n" +
                    "At 16th level, you can maximize your next spell as though using the Maximize Spell feat. Maximizing costs 6 charges of metamagic mastery.\n" +
                    "At 20th level, you can quicken your next spell as though using the Quicken Spell feat. Quickening costs 8 charges of metamagic mastery.\n" +
                    "The wizard has 1 metamagic mastery charge per 2 levels and recovers 1 charge at the start of each round."
                )
                .Configure();
        }
    }
}
