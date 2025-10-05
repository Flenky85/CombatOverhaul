using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.EntitySystem.Stats;

namespace CombatOverhaul.Patches.Weapon
{
    [HarmonyPatch(typeof(RuleCalculateWeaponStats), nameof(RuleCalculateWeaponStats.OnTrigger))]
    internal static class RemoveStrDamageBonus
    {
        private static void Prefix(RuleCalculateWeaponStats __instance)
        {
            if (__instance == null) return;
            if (__instance.Weapon == null) return;

            var stat = __instance.DamageBonusStat; 
            if (stat.HasValue && stat.Value != StatType.Strength) return;

            __instance.OverrideDamageBonusStat(StatType.Strength);
            __instance.OverrideDamageBonus = 0f;
        }
    }
}
