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
    internal static class SlowHalfling
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.SlowHalfling);

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
                "Halfling have a base speed of 15 feet.";

            feat.SetDescription(enText);
        }
    }
}
