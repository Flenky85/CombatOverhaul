using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Patches.Magic
{
    /// Desactiva la Spell Resistance globalmente (sin tocar inmunidades duras).
    [HarmonyPatch(typeof(RuleSpellResistanceCheck))]
    internal static class Patch_DisableSR_Minimal
    {
        // HasSpellResistance => false en todos los casos
        [HarmonyPatch(nameof(RuleSpellResistanceCheck.HasSpellResistance), MethodType.Getter)]
        [HarmonyPrefix]
        static bool HasSpellResistance_Prefix(ref bool __result)
        {
            __result = false;
            return false; // bloquea el getter original
        }
    }
}
