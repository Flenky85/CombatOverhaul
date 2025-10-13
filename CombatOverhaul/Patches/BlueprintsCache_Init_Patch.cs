using CombatOverhaul.Features;
using CombatOverhaul.Resources;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;

namespace CombatOverhaul.Patches
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class Patch_BlueprintsCache_Init
    {
        static void Postfix()
        {
            MonsterArmorMarkers.Register();
            ManaResourceBP.Register();

            UI.Mana.ManaUI.SetManaResource(ManaResourceBP.Mana);
        }
    }
}
