using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Clreic
{
    [AutoRegister]
    internal class ChannelEnergyFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ChannelEnergyFeature)
                .SetDescriptionValue(
                    "A good cleric (or a neutral cleric who worships a good deity) channels positive energy and can choose to deal " +
                    "damage to undead creatures or to heal living creatures.\n" +
                    "Channeling energy causes a burst that either heals all living creatures or damages all undead creatures in a " +
                    "30 - foot radius centered on the cleric. The amount of damage dealt or healed is equal to 1d6 points of damage " +
                    "+plus 1d6 points of damage for every two cleric levels beyond 1st(2d6 at 3rd, 3d6 at 5th, and so on). Creatures " +
                    "that take damage from channeled energy receive a Will save to halve the damage. The DC of this save is equal to " +
                    "10 + 1 / 2 the cleric's level + the cleric's Charisma modifier. Creatures healed by channel energy cannot exceed " +
                    "their maximum hit point total — all excess healing is lost. " +
                    "Activating this ability expends 6 charges. The cleric has a number of charges equal to " +
                    "6 plus her Charisma modifier. At the start of each of her turns, she regains 1.\n" +
                    "This is a standard action that does not provoke an attack of opportunity. A cleric can choose whether or not to include herself in this effect.\n" +
                    "A good cleric(or a neutral cleric of a good deity) can also channel stored spell energy into healing spells that she did " +
                    "not prepare ahead of time. The cleric can 'lose' any prepared spell that is not an orison or domain spell in order to " +
                    "cast any cure spell of the same spell level or lower(a cure spell is any spell with 'cure' in its name)."
                )
                .Configure();
        }
    }
}
