using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Races
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class SlowGnome
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.SlowGnome);
            if (feat == null) return;

            FeatureConfigurator.For(feat)
                .EditComponent<AddStatBonus>(c =>
                {
                    if (c.Stat == StatType.Speed && c.Descriptor == ModifierDescriptor.Racial)
                        c.Value = -5;
                })
                .SetDescriptionValue(
                    "Gnome have a base speed of 15 feet.")
                .Configure();
        }
    }
}
