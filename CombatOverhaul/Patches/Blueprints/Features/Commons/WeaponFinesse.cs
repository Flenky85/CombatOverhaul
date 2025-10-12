using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;

namespace CombatOverhaul.Patches.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class WeaponFinesse
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.WeaponFinesse);
            if (feat == null) return;

            var comps = new List<BlueprintComponent>(feat.ComponentsArray);
            for (int i = comps.Count - 1; i >= 0; i--)
            {
                if (comps[i] is AttackStatReplacement)        
                    comps.RemoveAt(i);
                else if (comps[i] is WeaponParametersAttackBonus) 
                    comps.RemoveAt(i);
            }

            feat.ComponentsArray = comps.ToArray();

            var pack = LocalizationManager.CurrentPack;
            if (pack != null)
            {
                var enText =
                    "With a light weapon, " +
                    "elven curve blade, estoc, or rapier made for a creature of your category, " +
                    "your primary-hand attacks convert 5% extra damage per point of Strength bonus " +
                    "into 5% extra damage per point of Dexterity bonus.";

                feat.SetDescription(enText);
            }
        }
    }
}
