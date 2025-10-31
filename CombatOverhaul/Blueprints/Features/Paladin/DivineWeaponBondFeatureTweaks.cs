using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class DivineWeaponBondFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.DivineWeaponBond)
                .SetDescriptionValue(
                    "Upon reaching 5th level, a paladin forms a divine bond with her weapon. As a swift action, " +
                    "she can call upon the aid of a celestial spirit for 4 rounds.\n" +
                    "At 5th level, this spirit grants the weapon a + 1 enhancement bonus.For every three levels beyond " +
                    "5th, the weapon gains another + 1 enhancement bonus, to a maximum of + 6 at 20th level.These bonuses " +
                    "can be added to the weapon, stacking with existing weapon bonuses to a maximum of + 5.\n" +
                    "Alternatively, they can be used to add any of the following weapon properties: axiomatic, brilliant " +
                    "energy, disruption, flaming, flaming burst, holy, keen, and speed.Adding these properties consumes an " +
                    "amount of bonus equal to the property's cost. These bonuses are added to any properties the weapon " +
                    "already has, but duplicate abilities don't stack.\n" +
                    "Divne weapon bond uses charges; activating this ability expends 3 charges. The paladin begins with 3 " +
                    "charges, and at 8th level and every 4 levels thereafter she gains 1 additional charge. " +
                    "At the start of each round, the paladin regains 1 charge, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
