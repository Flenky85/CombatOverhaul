using HarmonyLib;
using Kingmaker.Blueprints.Items.Armors;

namespace CombatOverhaul.Armor.Patch
{
    [HarmonyPatch(typeof(BlueprintItemArmor), "get_ArmorBonus")]
    internal static class ArmorBaseAC_Remove
    {
        static void Postfix(BlueprintItemArmor __instance, ref int __result)
        {
            if (!__instance.IsShield)
                __result = 0;
        }
    }
}
