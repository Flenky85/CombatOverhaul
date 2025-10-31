using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    [AutoRegister]
    internal static class GreaterTwoWeaponFightingFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.GreaterTwoWeaponFighting)
                .RemoveComponents(c => c is AddFacts)
                .SetDescriptionValue(
                    "Upgrades Improved Two-Weapon Fighting: your off-hand attacks with finesse weapons gain +5% damage per point of Dexterity bonus.")
                .Configure();
        }
    }
}
