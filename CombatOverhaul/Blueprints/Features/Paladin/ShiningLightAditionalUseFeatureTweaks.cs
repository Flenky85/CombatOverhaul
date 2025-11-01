using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class ShiningLightAditionalUseFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ShiningLightAditionalUse)
                .EditComponent<IncreaseResourceAmount>(c => { c.Value = 3; })
                .Configure();
        }
    }
}
