using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Progression.Wizard
{
    [AutoRegister]
    internal class SpecialisationSchoolNecromancyProgressionTweaks
    {
        public static void Register()
        {
            ProgressionConfigurator.For(ProgressionGuids.SpecialisationSchoolNecromancyProgression)
                .SetDescriptionValue(
                    "The dread and feared necromancer commands undead and uses the foul power of unlife against his enemies.\n" +
                    "Power over Undead: You can, as a standard action, use one of your uses of channel positive energy to cause all undead within 30 " +
                    "feet of you to flee, as if frightened. Undead receive a Will save to negate the effect. The DC for this Will " +
                    "save is equal to 10 + half your wizard level + your Charisma modifier. Undead that fail their save flee for 1d2 rounds. " +
                    "Undead receive a new saving throw each round to end the effect. If you use channel energy in this way, it has no other " +
                    "effect (it does not heal or harm nearby creatures). " +
                    "This ability have a cooldown of 4 rounds.\n" +
                    "Grave Touch: As a standard action, you can make a melee touch attack that causes a living creature to become shaken " +
                    "for a number of rounds equal to 1/2 your wizard level (minimum 1). If you touch a shaken creature with " +
                    "this ability, it becomes frightened for 1 round if it has fewer Hit Dice than your wizard level.\n" +
                    "Life Sight: At 8th level, you gain blindsight to a range of 10 feet. This ability only allows you to detect living creatures and undead " +
                    "creatures. This sight also tells you whether a creature is living or undead. Constructs and other creatures that are neither " +
                    "living nor undead cannot be seen with this ability. The range of this ability increases by 10 feet at 12th level, and by an " +
                    "additional 10 feet for every four levels beyond 12th." +
                    "Change shape starts with 3 charges and gains 1 additional charge every 5 levels; you can spend 1 charge for each round " +
                    "the ability is active, and you regain 1 charge each round while the ability is not active."
                )
                .Configure();
        }
    }
}
