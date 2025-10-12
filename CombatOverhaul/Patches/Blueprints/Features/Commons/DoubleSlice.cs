using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Patches.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class DoubleSlicePatch
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            FeatureConfigurator.For(FeaturesGuids.DoubleSlice)
                .RemoveComponents(c =>
                {
                    return c is AddMechanicsFeature amf
                           && amf.m_Feature == AddMechanicsFeature.MechanicsFeatureType.DoubleSlice;
                })
                .SetDescriptionValue("Off - hand only.Your off - hand weapon attacks gain + 5 % damage per point of Strength bonus.")
                .Configure();
        }
    }
}
