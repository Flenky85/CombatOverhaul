using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Witch
{
    [AutoRegister]
    internal class HexChannelerChannelSelectionTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.HexChannelerChannelSelection)
                .SetDescriptionValue(
                    "At 2nd level, a hex channeler can call upon her patron to release a wave of energy from herself. " +
                    "A good witch channels positive energy (like a good cleric), and an evil witch channels negative energy " +
                    "(like an evil cleric). A witch who is neither good nor evil must choose whether she channels positive or " +
                    "negative energy; once this choice is made, it cannot be reversed.\n" +
                    "Channeling energy causes a burst that affects all creatures of one type(either undead or living) in a 30 - " +
                    "foot radius centered on the witch. The hex channeler uses her witch " +
                    "level as her cleric level for all other effects dependent upon channel energy (except increasing the amount of " +
                    "damage healed or dealt).The hex channeler can choose whether or not to include herself or her familiar in this effect.\n" +
                    "This burst heals or deals 1d6 points of damage. Every time the hex channeler is able to learn a new hex(including major or grand hexes), " +
                    "she can instead increase her channel energy amount by 1d6.\n" +
                    "Activating this ability expends 6 charges. The shaman has a number of charges equal to " +
                    "6 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
