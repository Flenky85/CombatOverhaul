/*using System;
using System.Collections;
using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using UnityEngine; // Mathf

namespace CombatOverhaul.Patches
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

                // Limite real de DEX del ítem (post-mithral/feats/etc.)
                int? dexLimiter = armorEntity.DexBonusLimeterAC?.Value;

                // Fallback: leer del propio AC (limiter SourceType.Armor)
                if (!dexLimiter.HasValue)
                {
                    var f = AccessTools.Field(typeof(ModifiableValueArmorClass), "m_BaseAttributeBonusLimiters");
                    var list = f?.GetValue(__instance) as IEnumerable;
                    if (list != null)
                    {
                        foreach (var it in list)
                        {
                            var t = it.GetType();
                            var src = (ModifiableValueArmorClass.DexBonusLimiter.SourceType)
                                      AccessTools.Property(t, "Source").GetValue(it, null);
                            if (src == ModifiableValueArmorClass.DexBonusLimiter.SourceType.Armor)
                            {
                                dexLimiter = (int)AccessTools.Property(t, "Value").GetValue(it, null);
                                break;
                            }
                        }
                    }
                }

                // Último fallback heurístico por grupo
                int dexMax = Mathf.Clamp(dexLimiter ?? GuessDexMaxByArmorGroup(armorEntity), 0, 8);

                // porcentaje = 27 - 3 * dexMax
                int percent = Mathf.Clamp(27 - 3 * dexMax, 0, 27);
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
            catch {  }
        }

        private static int GuessDexMaxByArmorGroup(ItemEntityArmor armorEntity)
        {
            try
            {
                var pg = armorEntity.Blueprint?.ProficiencyGroup.ToString();
                switch (pg)
                {
                    case "Light": return 6;
                    case "Medium": return 3;
                    case "Heavy": return 1;
                    default: return 6;
                }
            }
            catch { return 6; }
        }
    }
}*/
