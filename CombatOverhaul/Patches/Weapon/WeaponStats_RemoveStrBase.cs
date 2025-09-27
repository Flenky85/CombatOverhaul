using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Enums;               // StatType
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;

namespace CombatOverhaul.Patches.Weapon
{
    /// Anula por completo el aporte base de Fuerza al daño (incluye penalizador en arcos).
    [HarmonyPatch(typeof(RuleCalculateWeaponStats), nameof(RuleCalculateWeaponStats.OnTrigger))]
    internal static class WeaponStats_RemoveStrBase
    {
        static void Prefix(RuleCalculateWeaponStats __instance)
        {
            if (__instance == null) return;
            var wbp = __instance.Weapon?.Blueprint;
            if (wbp == null) return;

            // Neutraliza solo cuando el stat que iba a aportar es Fuerza,
            // o cuando sería nulo (caso arcos normales que aplican penalizador por STR negativa).
            if (__instance.DamageBonusStat == StatType.Strength || __instance.DamageBonusStat == null)
            {
                // Forzamos que el stat sea STR (evita las ramas “stat nulo” de arco)
                __instance.OverrideDamageBonusStat(StatType.Strength);
                // Y multiplicador 0 ⇒ ni suma ni resta nada por STR (x0)
                __instance.OverrideDamageBonus = 0f;

                // (No tocamos AdditionalDamageBonusStatMultiplier; no hace falta.)
            }
        }
    }
}
