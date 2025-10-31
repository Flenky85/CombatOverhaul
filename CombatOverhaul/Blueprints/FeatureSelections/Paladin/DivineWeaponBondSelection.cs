using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.FeatureSelections.Paladin
{
    [AutoRegister]
    internal class DivineWeaponBondSelection
    {
        public static void Register()
        {
            FeatureSelectionConfigurator.For(FeaturesSelectionsGuids.PaladinDivineBondSelection)
                .SetDescriptionValue(
                    "Upon reaching 5th level, a paladin forms a divine bond with her god. This bond can take one of " +
                    "two forms. Once the form is chosen, it cannot be changed.\n" +
                    "The first type of bond allows a paladin to form a divine bond with her weapon.As a swift action, " +
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
                    "At the start of each round, the paladin regains 1 charge, up to her maximum number of charges.\n" +
                    "The second type of bond allows a paladin to gain the service of an unusually intelligent, strong, and " +
                    "loyal steed to serve her in her crusade against evil.This mount is a heavy horse.This mount functions as " +
                    "a druid's animal companion, using the paladin's level as her effective druid level.\n" +
                    "At 11th level, the mount gains the celestial creature simple template.\n" +
                    "At 15th level, a paladin's mount gains spell resistance equal to the paladin's level + 11."
                )
                .Configure();
        }
    }
}
