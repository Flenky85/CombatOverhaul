using HarmonyLib;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Armor.Patch
{
    [HarmonyPatch(typeof(RuleCalculateArmorMaxDexBonusLimit), nameof(RuleCalculateArmorMaxDexBonusLimit.OnTrigger))]
    internal static class NoMaxDexCap_Rule
    {
        static bool Prefix(RuleCalculateArmorMaxDexBonusLimit __instance)
        {
            __instance.Result = null; 
            return false;             
        }
    }
}
