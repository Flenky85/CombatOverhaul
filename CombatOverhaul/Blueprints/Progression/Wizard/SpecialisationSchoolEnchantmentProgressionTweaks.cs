using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Progression.Wizard
{
    [AutoRegister]
    internal class SpecialisationSchoolEnchantmentProgressionTweaks
    {
        public static void Register()
        {
            ProgressionConfigurator.For(ProgressionGuids.SpecialisationSchoolEnchantmentProgression)
                .SetDescriptionValue(
                    "The enchanter uses magic to control and manipulate the minds of his victims.\n" +
                    "Enchanting Smile: You gain a + 2 enhancement bonus on Persuasion checks and saving throws against Enchantment spells. " +
                    "This bonus increases by + 1 for every five wizard levels you possess, up to a maximum of + 6 at 20th level.\n" +
                    "Dazing Touch: You can cause a living creature to become dazed for 1 round as a melee touch attack. Creatures with " +
                    "more Hit Dice than your wizard level are unaffected.\n" +
                    "Aura of Despair: At 8th level, you can emit a 30 - foot aura of despair for a number of rounds per day equal to your " +
                    "wizard level. Enemies within this aura take a –2 penalty on ability checks, attack rolls, damage rolls, saving throws, " +
                    "and skill checks. This is a mind - affecting effect." +
                    "Foretell starts with 3 charges and gains 1 additional charge every 5 levels; you can spend 1 charge for each round " +
                    "the ability is active, and you regain 1 charge each round while the ability is not active."
                )
                .Configure();
        }
    }
}
