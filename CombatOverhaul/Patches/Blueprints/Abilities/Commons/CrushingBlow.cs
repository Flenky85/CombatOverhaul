using CombatOverhaul.Guids;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;

namespace CombatOverhaul.Patches.Blueprints.Abilities.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class CrushingBlow_ActionCost
    {
        private static bool _done;

        static void Postfix()
        {
            if (_done) return; _done = true;

            var ability = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(AbilitiesGuids.CrushingBlow);
            if (ability == null) return;

            AbilityConfigurator.For(AbilitiesGuids.CrushingBlow)
                .OnConfigure(bp => bp.m_IsFullRoundAction = false)
                .Configure();
        }
    }
}
