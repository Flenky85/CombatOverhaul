/*using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using CombatOverhaul.Combat.Calculators;

namespace CombatOverhaul.Patches.Weapon
{
    [HarmonyPatch(typeof(RuleCalculateWeaponStats), nameof(RuleCalculateWeaponStats.OnTrigger))]
    internal static class WeaponStats_StrAsPercent_UIFriendly
    {
        static void Prefix(RuleCalculateWeaponStats __instance)
        {
            var actor = __instance != null ? __instance.Initiator : null;
            var wep = __instance != null ? __instance.Weapon : null;
            if (actor == null || wep == null) return;

            float perPoint = StrengthDamageCalculator.ComputeOverrideDamagePerPoint(actor, wep);
            if (perPoint > 0f)
            {
                __instance.OverrideDamageBonus = perPoint;
            }
        }
    }
}*/
