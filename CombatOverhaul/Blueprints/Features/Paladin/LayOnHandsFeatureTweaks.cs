using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class LayOnHandsTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.LayOnHands)
                .SetDescriptionValue(
                    "Beginning at 2nd level, a paladin can heal wounds (her own or those of others) by " +
                    "touch. With one use of this ability, a paladin can heal 1d6 hit points of damage for " +
                    "every two paladin levels she possesses. Using this ability is a standard action, unless " +
                    "the paladin targets herself, in which case it is a swift action.\n " +
                    "Alternatively, a paladin can use this healing power to deal damage to undead creatures, " +
                    "dealing 1d6 points of damage for every two levels the paladin possesses. Using lay on hands " +
                    "in this way requires a successful melee touch attack and doesn't provoke an attack of " +
                    "opportunity. Undead do not receive a saving throw against this damage.\n" +
                    "Lay on hands uses charges; activating this ability expends 3 charges. The paladin begins with 3 " +
                    "charges, and at 2nd level and every 2 levels thereafter she gains 1 additional charge; she also " +
                    "adds her Cha bonus (if any) to her maximum number of charges. At the start of each round, the " +
                    "paladin regains 1 charge, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
