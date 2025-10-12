using System.Linq;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;

namespace CombatOverhaul.Patches.Weapon
{
    [HarmonyPatch(typeof(RuleCalculateWeaponStats), nameof(RuleCalculateWeaponStats.OnTrigger))]
    internal static class RemoveAllStrDamage
    {
        private static void Prefix(RuleCalculateWeaponStats __instance)
        {
            if (__instance?.Weapon == null) return;

            if (__instance.DamageBonusStat.HasValue &&
                __instance.DamageBonusStat.Value == StatType.Strength)
            {
                __instance.DamageBonusStat = null;
                __instance.DamageBonusStatMultiplier = 0f;
                __instance.AdditionalDamageBonusStatMultiplier = 0f;
            }
        }
    }
}
