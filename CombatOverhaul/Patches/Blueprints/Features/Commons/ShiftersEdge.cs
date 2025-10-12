using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using System;
using System.Linq;

namespace CombatOverhaul.Patches.Blueprints.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class ShiftersEdge
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.ShiftersEdge);
            if (feat == null) return;

            var triggers = feat
                .GetComponents<Kingmaker.UnitLogic.Mechanics.Components.AddInitiatorAttackRollTrigger>()
                ?.ToArray();
            if (triggers == null || triggers.Length == 0) return;

            foreach (var t in triggers)
            {
                var actions = t.Action?.Actions;
                if (actions == null || actions.Length == 0) continue;

                int idx = Array.FindIndex(actions, a => a is Conditional);
                if (idx < 0) continue;

                var cond = (Conditional)actions[idx];
                var trueActions = cond.IfTrue?.Actions ?? Array.Empty<GameAction>();

                var newList = actions.ToList();
                newList.RemoveAt(idx);
                if (trueActions.Length > 0) newList.InsertRange(idx, trueActions);

                t.OnlyHit = true; 
                t.Action = new ActionList { Actions = newList.ToArray() };
            }

            var pack = LocalizationManager.CurrentPack;
            if (pack != null)
            {
                var enText = "You use your shapechanging powers to make your natural attacks especially lethal.\n" +
                    "You also add half your shifter level to the damage. This bonus applies whenever you make a melee " +
                    "attack with your claws or with a natural attack augmented by your claws.";

                feat.SetDescription(enText);
            }
        }
    }
}
