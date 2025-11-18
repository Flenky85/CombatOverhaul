using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Witch
{
    [AutoRegister]
    internal class HagboundWitchVileCurseFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.HagboundWitchVileCurseFeature)
                .SetDescriptionValue(
                    "At 10th level, the hagbound becomes exceptionally skilled at cursing others. She can cast " +
                    "bestow curse at will, but its duration becomes once per combat. Once a creature has been " +
                    "affected by this ability, it is immune to the witch's vile curse ability for once per combat."
                )
                .Configure();
        }
    }
}
