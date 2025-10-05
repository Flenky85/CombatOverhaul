using HarmonyLib;
using Kingmaker.EntitySystem.Entities;  
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using UnityEngine;                      
using Kingmaker.UnitLogic;
using CombatOverhaul.Calculators;

namespace CombatOverhaul.Patches.Armor
{
    [HarmonyPatch(typeof(ModifiableValueArmorClass), "OnUpdate")]
    internal static class AC_UniversalReduction
    {
        private const int MaxDexClamp = 8;

        [HarmonyPostfix, HarmonyPriority(Priority.Last)]
        private static void Postfix(ModifiableValueArmorClass __instance)
        {
            try
            {
                var unit = __instance?.Stats?.Owner?.Unit;
                if (unit == null) return;

                int percent = GetReductionPercent(unit, __instance);
                if (percent <= 0) return;

                __instance.Touch = Reduce(__instance.Touch, percent);
                __instance.FlatFootedTouch = Reduce(__instance.FlatFootedTouch, percent);
                __instance.FlatFooted = Reduce(__instance.FlatFooted, percent);
                __instance.ModifiedValue = Reduce(__instance.ModifiedValue, percent);
            }
            catch
            {
                
            }
        }

        private static int GetReductionPercent(UnitEntityData unit, ModifiableValueArmorClass ac)
        {
            if (unit.Body?.Armor?.MaybeItem is ItemEntityArmor armorEntity)
            {
                int dexMax = ResolveArmorMaxDex(armorEntity, ac);
                return ArmorCalculator.ComputeAcReductionPercentFromMaxDex(dexMax);
            }

            var desc = unit.Descriptor;
            if (desc != null)
            {
                var heavyRef = CombatOverhaul.Utils.MarkerRefs.HeavyRef;
                var mediumRef = CombatOverhaul.Utils.MarkerRefs.MediumRef;

                if (heavyRef != null && desc.HasFact(heavyRef)) return 24;
                if (mediumRef != null && desc.HasFact(mediumRef)) return 12;
            }

            return 0;
        }

        private static int ResolveArmorMaxDex(ItemEntityArmor armor, ModifiableValueArmorClass ac)
        {
            int? dexLimiter = armor.DexBonusLimeterAC != null
                ? (int?)armor.DexBonusLimeterAC.Value
                : null;

            if (!dexLimiter.HasValue)
            {
                var limiters = ac.m_BaseAttributeBonusLimiters;
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

            int raw = dexLimiter ?? ArmorCalculator.GetArmorMaxDex(armor);
            return Mathf.Clamp(raw, 0, MaxDexClamp);
        }

        private static int Reduce(int v, int pct)
        {
            if (v <= 0 || pct <= 0) return v;
            int r = (v * pct + 99) / 100;
            if (r <= 0) r = 1;
            int res = v - r;
            return res < 0 ? 0 : res;
        }
    }
}
