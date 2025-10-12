using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;

namespace CombatOverhaul.Patches.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class ImprovedTwoWeaponFighting
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            FeatureConfigurator.For(FeaturesGuids.ImprovedTwoWeaponFighting)
                .RemoveComponents(c => c is AddFacts)
                .SetDescriptionValue(
                    "Off-hand only. When using finesse weapons, your off-hand attacks gain +2.5% damage per point of Dexterity bonus (replacing Strength-based scaling).")
                .Configure();
        }
    }
}
