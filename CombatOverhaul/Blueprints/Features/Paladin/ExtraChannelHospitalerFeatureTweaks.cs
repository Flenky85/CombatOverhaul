using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class ExtraChannelHospitalerFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ExtraChannelHospitaler)
                .EditComponent<IncreaseResourceAmount>(c => c.Value =3)
                .SetDescriptionValue(
                    "You gain 3 additional channel energy charges."
                )
                .Configure();
        }
    }
}
