using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Assets.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Visual.Decals;
using System.Runtime.Remoting.Messaging;
using static Kingmaker.Blueprints.BlueprintAbilityResource;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class ChannelPositiveEnergyFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.LayOnHands)
                .SetDescriptionValue(
                    "When a paladin reaches 4th level, she gains the supernatural ability to channel " +
                    "positive energy like a cleric. Using this ability consumes two uses of her lay on " +
                    "hands ability. This energy can be used to cause or heal damage, depending on the " +
                    "creatures targeted.\n" +
                    "A paladin channels positive energy and can choose to deal damage to undead creatures " +
                    "or to heal living creatures.\n" +
                    "Channeling positive energy causes a burst that affects all creatures of one type(either " +
                    "undead or living) in a 30 - foot radius centered on the paladin.The amount of damage " +
                    "dealt or healed is equal to 1d6 points of damage plus 1d6 points of damage for every " +
                    "two paladin levels beyond 1st (2d6 at 4th, 3d6 at 5th, and so on). Creatures that take " +
                    "damage from channeled energy receive a Will save to halve the damage. The DC of this " +
                    "save is equal to 10 + 1/2 the paladin's level + the paladin's Charisma modifier. " +
                    "Creatures healed by channel energy cannot exceed their maximum hit point total — all " +
                    "excess healing is lost.\n" +
                    "This is a standard action that does not provoke an attack of opportunity."
                )
                .Configure();
        }
    }
}
