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
    internal static class SlowAndSteadyOread
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.SlowAndSteadyOread);

            if (feat == null) return;

            foreach (var comp in feat.ComponentsArray)
            {
                if (comp is AddStatBonus asb
                    && asb.Stat == StatType.Speed
                    && asb.Descriptor == ModifierDescriptor.Armor)
                {
                    asb.Value = -5;
                }
            }
            var enText =
                "Oread have a base speed of 15 feet, but their speed is never modified by armor or encumbrance.";

            feat.SetDescription(enText);
        }
    }
}
