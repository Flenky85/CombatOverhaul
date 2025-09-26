using System;
using System.Collections;
using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using UnityEngine; // Mathf
using CombatOverhaul.Combat.Calculators; // ArmorCalculator
using CombatOverhaul.Utils;              // Log (opcional, si quieres cambiar Debug por Log)

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
                var unit = __instance != null && __instance.Stats != null && __instance.Stats.Owner != null
                    ? __instance.Stats.Owner.Unit
                    : null;
                if (unit == null) return;

                // Armadura equipada
                var armorEntity = unit.Body != null && unit.Body.Armor != null
                    ? unit.Body.Armor.Item as ItemEntityArmor
                    : null;
                if (armorEntity == null) return;

                // Limite real de DEX del ítem (post-mithral/feats/etc.)
                int? dexLimiter = armorEntity.DexBonusLimeterAC != null
                    ? (int?)armorEntity.DexBonusLimeterAC.Value
                    : null;

                // Fallback: leer del propio AC (limiter SourceType.Armor)
                if (!dexLimiter.HasValue)
                {
                    var f = AccessTools.Field(typeof(ModifiableValueArmorClass), "m_BaseAttributeBonusLimiters");
                    var list = f != null ? f.GetValue(__instance) as IEnumerable : null;
                    if (list != null)
                    {
                        foreach (var it in list)
                        {
                            var t = it.GetType();
                            var srcProp = AccessTools.Property(t, "Source");
                            var valProp = AccessTools.Property(t, "Value");
                            if (srcProp == null || valProp == null) continue;

                            var src = (ModifiableValueArmorClass.DexBonusLimiter.SourceType)srcProp.GetValue(it, null);
                            if (src == ModifiableValueArmorClass.DexBonusLimiter.SourceType.Armor)
                            {
                                dexLimiter = (int)valProp.GetValue(it, null);
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
                int Reduce(int v)
                {
                    if (v <= 0) return v;
                    int r = (v * percent + 99) / 100; // ceil
                    if (r <= 0) r = 1;
                    int res = v - r;
                    return res < 0 ? 0 : res;
                }

                // Reducimos todos los valores que usa juego + UI
                __instance.Touch = Reduce(__instance.Touch);
                __instance.FlatFootedTouch = Reduce(__instance.FlatFootedTouch);
                __instance.FlatFooted = Reduce(__instance.FlatFooted);
                __instance.ModifiedValue = Reduce(__instance.ModifiedValue); // AC total mostrada/consultada
            }
            catch
            {
                // silencioso para no ensuciar el log en combate
            }
        }
    }
}
