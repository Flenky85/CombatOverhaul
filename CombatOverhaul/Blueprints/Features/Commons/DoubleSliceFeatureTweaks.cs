using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    internal static class DoubleSliceFeatureTweaks
    {
        public static void Register()
        {
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
