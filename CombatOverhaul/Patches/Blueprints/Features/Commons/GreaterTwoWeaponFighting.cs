using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;

namespace CombatOverhaul.Patches.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class GreaterTwoWeaponFighting
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            FeatureConfigurator.For(FeaturesGuids.GreaterTwoWeaponFighting)
                .RemoveComponents(c => c is AddFacts)
                .SetDescriptionValue(
                    "Upgrades Improved Two-Weapon Fighting: your off-hand attacks with finesse weapons gain +5% damage per point of Dexterity bonus.")
                .Configure();
        }
    }
}
