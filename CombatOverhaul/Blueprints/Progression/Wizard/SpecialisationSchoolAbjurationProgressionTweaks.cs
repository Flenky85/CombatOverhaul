using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Progression.Wizard
{
    [AutoRegister]
    internal class SpecialisationSchoolAbjurationProgressionTweaks
    {
        public static void Register()
        {
            ProgressionConfigurator.For(ProgressionGuids.SpecialisationSchoolAbjurationProgression)
                .SetDescriptionValue(
                    "The abjurer uses magic against itself, and masters the art of defensive and warding magics.\n "+
                    "Resistance: You gain resistance 5 to an energy type of your choice, chosen when you prepare spells. " +
                    "This resistance can be changed each combat. At 11th level, this resistance increases to 10.At 20th level, " +
                    "this resistance changes to immunity to the chosen energy type. \n" +
                    "Protective Ward: As a standard action, you can create a 10 - foot - radius field of protective magic centered on you that lasts " +
                    "for a number of rounds equal to your Intelligence modifier. All allies in this area (including you) receive a " +
                    "+1 deflection bonus to their Armor Class. This bonus increases by +1 for every five wizard levels you possess. " +
                    "The aura have a cooldown of 3 rounds.\n" +
                    "Energy Absorption: At 6th level, you gain an amount of energy absorption equal to 3 times your wizard level per combat. " +
                    "Whenever you take energy damage, apply immunity, vulnerability(if any), and resistance first and apply the rest to this absorption, " +
                    "reducing your daily total by that amount. Any damage in excess of your absorption is applied to you normally."
                )
                .Configure();
        }
    }
}
