using HarmonyLib;
using Kingmaker.Blueprints.Items.Armors;

namespace CombatOverhaul.Patches
{
    /// <summary>
    /// Elimina el bono base de AC de las armaduras (mantiene el de los escudos).
    /// </summary>
    [HarmonyPatch(typeof(BlueprintItemArmor), "get_ArmorBonus")]
    internal static class ArmorBaseAC_Remove
    {
        static void Postfix(BlueprintItemArmor __instance, ref int __result)
        {
            // Solo armaduras: si es escudo, dejamos su bono base intacto
            if (!__instance.IsShield)
                __result = 0;
        }
    }
}
