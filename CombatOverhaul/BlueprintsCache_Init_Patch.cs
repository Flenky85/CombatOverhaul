using CombatOverhaul.Blueprints;
using CombatOverhaul.Magic.UI.ManaDisplay;
using CombatOverhaul.Features;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;

namespace CombatOverhaul
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    [HarmonyPriority(Priority.Last)]
    internal static class Patch_BlueprintsCache_Init
    {
        private static bool _initialized;

        static void Postfix()
        {
            if (_initialized) return;
            _initialized = true;

            
            MonsterArmorMarkers.Register();
            ManaResource.Register();
            ManaUI.SetManaResource(ManaResource.Mana);

            BlueprintsAutoRegistrar.RunAll();
        }
    }
}
