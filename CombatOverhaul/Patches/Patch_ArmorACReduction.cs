using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.EntitySystem.Stats;
using UnityEngine; // Mathf

namespace CombatOverhaul.Patches
{
    [HarmonyPatch(typeof(RuleCalculateAC), "OnTrigger")]
    internal static class Patch_ArmorACReduction
    {
        [HarmonyPostfix]
        private static void Postfix(RuleCalculateAC __instance)
        {
            try
            {
                if (__instance == null) return;
                var unit = __instance.Target;
                if (unit == null) return;

                int totalAC = __instance.Result;
                if (totalAC <= 0) return;

                // Armadura equipada
                ItemEntity armorItem = unit.Body?.Armor?.Item;
                var armorEntity = armorItem as Kingmaker.Items.ItemEntityArmor;
                if (armorEntity == null) return;

                // Limiter real del ítem; fallback: leer del AC si aún no está aplicado
                int? dexLimiter = armorEntity.DexBonusLimeterAC?.Value;
                if (!dexLimiter.HasValue)
                {
                    var ac = unit.Stats?.AC;
                    var f = AccessTools.Field(typeof(ModifiableValueArmorClass), "m_BaseAttributeBonusLimiters");
                    var list = f?.GetValue(ac) as System.Collections.IEnumerable;
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

                // Porcentaje = 27 - 3*dexMax
                int percent = Mathf.Clamp(27 - 3 * dexMax, 0, 27);
                if (percent <= 0) return;

                // Reducción = ceil(totalAC * percent / 100), mínimo 1 si percent>0
                int reduction = (totalAC * percent + 99) / 100;
                if (reduction <= 0) reduction = 1;

                __instance.Result = Math.Max(0, totalAC - reduction);
            }
            catch
            {
                // Silencioso
            }
        }

        private static int GuessDexMaxByArmorGroup(Kingmaker.Items.ItemEntityArmor armorEntity)
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
}
