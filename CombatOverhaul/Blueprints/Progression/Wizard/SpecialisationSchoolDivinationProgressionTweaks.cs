using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Progression.Wizard
{
    [AutoRegister]
    internal class SpecialisationSchoolDivinationProgressionTweaks
    {
        public static void Register()
        {
            ProgressionConfigurator.For(ProgressionGuids.SpecialisationSchoolDivinationProgression)
                .SetDescriptionValue(
                    "Diviners are masters of remote viewing, prophecies, and using magic to explore the world.\n" +
                    "Forewarned: You receive a bonus on initiative checks equal to 1 / 2 your wizard level (minimum + 1). " +
                    "At 20th level, anytime you roll initiative, assume the roll resulted in a natural 20.\n" +
                    "Diviner's Fortune: When you activate this school power, you can touch any creature as a standard action " +
                    "to give it an insight bonus on all of its attack rolls, skill checks, ability checks, and saving throws equal " +
                    "to 1/2 your wizard level (minimum +1) for 1 round.\n" +
                    "Foretell: At 8th level, you can utter a prediction of the immediate future.While your foretelling is in effect, " +
                    "you emit a 30 - foot aura of fortune that aids your allies or hinders your enemies, as chosen by you at the time " +
                    "of prediction.If you choose to aid, you and your allies gain a + 2 luck bonus on ability checks, attack rolls, " +
                    "caster level checks, saving throws, and skill checks.If you choose to hinder, your enemies take a –2 penalty on " +
                    "those rolls instead. " +
                    "Foretell starts with 3 charges and gains 1 additional charge every 5 levels; you can spend 1 charge for each round " +
                    "the ability is active, and you regain 1 charge each round while the ability is not active."
                )
                .Configure();
        }
    }
}
