using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Progression.Wizard
{
    [AutoRegister]
    internal class SpecialisationSchoolTransmutationProgressionTweaks
    {
        public static void Register()
        {
            ProgressionConfigurator.For(ProgressionGuids.SpecialisationSchoolTransmutationProgression)
                .SetDescriptionValue(
                    "Transmuters use magic to change the world around them.\n" +
                    "Physical Enhancement: You gain a + 1 enhancement bonus to one physical ability score(Strength, " +
                    "Dexterity, or Constitution). This bonus increases by + 1 for every five wizard levels you possess " +
                    "to a maximum of + 5 at 20th level.At 20th level, this bonus applies to two physical ability scores " +
                    "of your choice.\n" +
                    "Telekinetic Fist: As a standard action, you can strike with a telekinetic fist, targeting any foe within " +
                    "30 feet as a ranged touch attack.The telekinetic fist deals 1d4 points of bludgeoning damage + 1 for every " +
                    "wizard levels you possess.\n" + 
                    "Change Shape: At 8th level, you can change your shape like beast shape II or elemental body I. " +
                    "At 12th level, this ability functions like beast shape III or elemental body II." +
                    "Change shape starts with 3 charges and gains 1 additional charge every 5 levels; you can spend 1 charge for each round " +
                    "the ability is active, and you regain 1 charge each round while the ability is not active."
                )
                .Configure();
        }
    }
}
