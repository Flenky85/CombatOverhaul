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
        private static void Postfix(RuleCalculateWeaponStats __instance)
        {
            if (__instance?.Weapon == null) return;

            var dd = __instance.DamageDescription.FirstOrDefault();
            if (dd == null) return;

            if (__instance.DamageBonusStat.HasValue &&
                __instance.DamageBonusStat.Value == StatType.Strength)
            {
                int str = __instance.Initiator?.Stats?.Strength?.Bonus ?? 0;

                float mult = __instance.DamageBonusStatMultiplier + __instance.AdditionalDamageBonusStatMultiplier;
                int applied = (int)(str * ((str < 0) ? 1f : mult));

                if (applied != 0)
                    dd.AddModifier(new Modifier(-applied, StatType.Strength)); 

                return; 
            }


            if (!__instance.DamageBonusStat.HasValue)
            {
                var cat = __instance.Weapon.Blueprint.Category;
                if (cat == WeaponCategory.Shortbow || cat == WeaponCategory.Longbow)
                {
                    int str = __instance.Initiator?.Stats?.Strength?.Bonus ?? 0;
                    if (str < 0)
                        dd.AddModifier(new Modifier(-str, StatType.Strength));
                }
            }
        }
    }
}
