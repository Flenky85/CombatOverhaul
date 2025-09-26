using System;
using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using UnityEngine; // Mathf
using CombatOverhaul.Combat.Calculators; // ArmorCalculator

namespace CombatOverhaul.Patches.Armor
{
    [HarmonyPatch(typeof(ModifiableValueArmorClass), "OnUpdate")]
    internal static class Patch_AC_UniversalReduction
    {
        [HarmonyPostfix, HarmonyPriority(Priority.Last)]
        private static void Postfix(ModifiableValueArmorClass __instance)
        {
            try
            {
                var unit = __instance?.Stats?.Owner?.Unit;
                if (unit == null) return;

                // Armadura equipada
                var armorEntity = unit.Body?.Armor?.Item as ItemEntityArmor;
                if (armorEntity == null) return;

                // Límite real de DEX aplicado por el ítem (si existe ya aplicado)
                int? dexLimiter = armorEntity.DexBonusLimeterAC != null
                    ? (int?)armorEntity.DexBonusLimeterAC.Value
                    : null;

                // Fallback: leer del propio AC el limitador con SourceType.Armor (sin reflexión)
                if (!dexLimiter.HasValue)
                {
                    var limiters = __instance.m_BaseAttributeBonusLimiters; // publicized
                    if (limiters != null)
                    {
                        foreach (var it in limiters)
                        {
                            if (it.Source == ModifiableValueArmorClass.DexBonusLimiter.SourceType.Armor)
                            {
                                dexLimiter = it.Value;
                                break;
                            }
                        }
                    }
                }

                // Último fallback heurístico por grupo
                int dexMax = Mathf.Clamp(dexLimiter ?? ArmorCalculator.GuessDexMaxByArmorGroup(armorEntity), 0, 8);

                // porcentaje = 27 - 3 * dexMax (cap 0..27)
                int percent = ArmorCalculator.ComputeUniversalAcReductionPercent(dexMax);
                if (percent <= 0) return;

                // Reducción = ceil(v * percent / 100), mínimo 1 si v>0
                static int Reduce(int v, int pct)
                {
                    if (v <= 0) return v;
                    int r = (v * pct + 99) / 100; // ceil
                    if (r <= 0) r = 1;
                    int res = v - r;
                    return res < 0 ? 0 : res;
                }

                // Reducimos todos los valores que usa juego + UI
                __instance.Touch = Reduce(__instance.Touch, percent);
                __instance.FlatFootedTouch = Reduce(__instance.FlatFootedTouch, percent);
                __instance.FlatFooted = Reduce(__instance.FlatFooted, percent);
                __instance.ModifiedValue = Reduce(__instance.ModifiedValue, percent); // AC total mostrada/consultada
            }
            catch
            {
                // silencioso para no ensuciar el log en combate
            }
        }
    }
}
