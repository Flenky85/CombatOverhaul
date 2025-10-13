using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;

namespace CombatOverhaul.Utils
{
    internal static class MarkerRefs
    {
        public static BlueprintFeature HeavyBp =>
            ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(CombatOverhaul.Features.MonsterArmorMarkers.HeavyGuid);

        public static BlueprintFeature MediumBp =>
            ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(CombatOverhaul.Features.MonsterArmorMarkers.MediumGuid);

        public static BlueprintFeatureReference HeavyRef => HeavyBp?.ToReference<BlueprintFeatureReference>();
        public static BlueprintFeatureReference MediumRef => MediumBp?.ToReference<BlueprintFeatureReference>();
    }
}
