using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Patches.Blueprints.Features.Races
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class SlowAndSteadyDwarf
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;
            
            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.SlowAndSteadyDwarf);

            FeatureConfigurator.For(feat)
                .EditComponent<AddStatBonus>(c =>
                {
                    if (c.Stat == StatType.Speed && c.Descriptor == ModifierDescriptor.Armor)
                        c.Value = -5;
                })
                .Configure();
            
            var enText =
                "Dwarves have a base speed of 15 feet, but their speed is never modified by armor or encumbrance.";

            feat.SetDescription(enText);
        }
    }
}
