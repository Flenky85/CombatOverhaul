using CombatOverhaul.Features;
using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;

namespace CombatOverhaul.Patches
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class Patch_BlueprintsCache_Init
    {
        static void Postfix()
        {
            try
            {
                Log.Info("[Markers] BlueprintsCache.Init Postfix -> Register markers");
                MonsterArmorMarkers.Register();
                Log.Info("[Markers] Register() OK");
            }
            catch (System.Exception ex)
            {
                Log.Error("[Markers] Register() FAILED", ex);
            }
        }
    }
}
