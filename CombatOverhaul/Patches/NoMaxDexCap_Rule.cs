using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Patches
{
    /// <summary>
    /// Desactiva el límite de DEX para la CA.
    /// Cualquier consulta de RuleCalculateArmorMaxDexBonusLimit devuelve null,
    /// por lo que NO se añade ningún DexBonusLimiter a la AC.
    /// Cubre armaduras y escudos (ambos usan el mismo flujo).
    /// </summary>
    [HarmonyPatch(typeof(RuleCalculateArmorMaxDexBonusLimit), nameof(RuleCalculateArmorMaxDexBonusLimit.OnTrigger))]
    internal static class NoMaxDexCap_Rule
    {
        static bool Prefix(RuleCalculateArmorMaxDexBonusLimit __instance)
        {
            __instance.Result = null; // sin límite
            return false;             // saltar el original
        }
    }
}
