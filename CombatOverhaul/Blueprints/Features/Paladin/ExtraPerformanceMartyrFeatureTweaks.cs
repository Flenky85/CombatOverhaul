using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class ExtraPerformanceMartyrFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ExtraPerformanceMartyr)
                .EditComponent<IncreaseResourceAmount>(c => c.Value = 1)
                .SetDescriptionValue(
                    "You gain 1 additional stigmata charge."
                )
                .Configure();
        }
    }
}
