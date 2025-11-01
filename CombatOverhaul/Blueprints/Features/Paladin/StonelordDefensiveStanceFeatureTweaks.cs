using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class StonelordDefensiveStanceFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.StonelordDefensiveStance)
                .SetDescriptionValue(
                    "At 4th level, a stonelord can enter a defensive stance, a position of readiness " +
                    "and trance-like determination. Temporary increases to " +
                    "Constitution, such as those gained from the defensive stance and spells like bear's " +
                    "endurance, do not increase the total number of rounds that the stonelord can maintain " +
                    "a defensive stance per day. The stonelord can enter a defensive stance as a free action. " +
                    " While in a defensive stance, " +
                    "a stonelord gains a +2 bonus on melee attack rolls, melee damage rolls, thrown weapon " +
                    "damage rolls, and Will saving throws. In addition, she gains a +2 dodge bonus to Armor " +
                    "Class and 2 temporary hit points per Hit Die. While in a defensive stance, a stonelord " +
                    "cannot willingly move from her current position through any means.\n" +
                    "The stonelord can end her defensive stance as a free action; after ending the stance, she is " +
                    "fatigued for a number of rounds equal to 2 times the number of rounds spent in the stance.A " +
                    "stonelord cannot enter a new defensive stance while fatigued or exhausted but can otherwise enter " +
                    "a stance multiple times during a single encounter or combat. If a stonelord falls unconscious, her " +
                    "defensive stance immediately ends, placing her in peril of death.A defensive stance requires " +
                    "a level of emotional calm, and it may not be maintained by a character in a rage(such as " +
                    "from the rage class feature or the rage spell).\n" +
                    "Defensive stance uses charges; activating this ability expends 1 charges per round. The paladin begins with 3 " +
                    "charges; she also adds her Con bonus (if any) to her maximum number of charges. At the start of " +
                    "her turn, if this ability is not active that round and she is not fatigued or exhausted, the paladin " +
                    "regains 1 charges, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
