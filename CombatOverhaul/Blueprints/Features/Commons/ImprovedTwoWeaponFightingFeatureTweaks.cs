using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    [AutoRegister]
    internal static class ImprovedTwoWeaponFightingFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ImprovedTwoWeaponFighting)
                .RemoveComponents(c => c is AddFacts)
                .SetDescriptionValue(
                    "Off-hand only. When using finesse weapons, your off-hand attacks gain +2.5% damage per point of Dexterity bonus (replacing Strength-based scaling).")
                .Configure();
        }
    }
}
