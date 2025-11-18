using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Progression.Wizard
{
    [AutoRegister]
    internal class SpecialisationSchoolEvocationProgressionTweaks
    {
        public static void Register()
        {
            ProgressionConfigurator.For(ProgressionGuids.SpecialisationSchoolEvocationProgression)
                .SetDescriptionValue(
                    "Evokers revel in the raw power of magic, and can use it to create and destroy with shocking ease.\n" +
                    "Intense Spells: Whenever you cast an evocation spell that deals hit point damage, add 1 / 2 your wizard " +
                    "level to the damage(minimum + 1). This bonus only applies once to a spell, not once per missile or ray, and " +
                    "cannot be split between multiple missiles or rays. This damage is of the same type as the spell. At 20th level, " +
                    "whenever you cast an Evocation spell, you can roll twice to penetrate a creature's spell resistance and " +
                    "take the better result.\n" +
                    "Force Missile: As a standard action, you can unleash a force missile that automatically strikes a foe, as magic missile. " +
                    "The force missile deals 1d4 + your Intense Spells bonus +1 per two wizard levels damage. " +
                    "This is a force effect.\n" + 
                    "Elemental Wall: At 8th level, one per day you can create a wall of energy. You can choose fire, cold, acid or electricity damage. " +
                    "The elemental wall otherwise functions like wall of fire. " +
                    "The ability have a cooldown of 4 rounds."
                )
                .Configure();
        }
    }
}
