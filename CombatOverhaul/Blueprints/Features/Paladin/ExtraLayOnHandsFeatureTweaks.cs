using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class ExtraLayOnHandsFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ExtraLayOnHands)
                .EditComponent<IncreaseResourceAmount>(c => c.Value = 3)
                .SetDescriptionValue(
                    "Benefit: You gain 3 additional Lay on Hands charges.\n" +
                    "Special: You can gain Extra Lay On Hands multiple times.Its effects stack."
                )
                .Configure();
        }
    }
}
