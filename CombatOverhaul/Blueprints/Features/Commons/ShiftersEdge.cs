using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class ShiftersEdge
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            FeatureConfigurator.For(FeaturesGuids.ShiftersEdge)
                .RemoveComponents(c => c is AddFacts)
                .SetDescriptionValue(
                    "You use your shapechanging powers to make your natural attacks especially lethal.\n" +
                    "If your Dexterity bonus is higher than your Strength bonus, your melee attacks with your natural attacks " +
                    "gain additional damage equal to +1% per point of your Dexterity bonus and +1% per point of your Strength bonus.")
                .Configure();
        }
    }
}
