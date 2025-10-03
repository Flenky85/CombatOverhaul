using System;
using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using UnityEngine; // Mathf
using CombatOverhaul.Combat.Calculators; // ArmorCalculator
using Kingmaker.UnitLogic;               // HasFact()

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

                int percent = 0;

                // 1) Armadura real (vía slot seguro)
                var armorSlot = unit.Body?.Armor;
                ItemEntityArmor armorEntity = null;
                if (armorSlot != null && armorSlot.HasArmor)
                    armorEntity = (ItemEntityArmor)armorSlot.MaybeItem;

                if (armorEntity != null)
                {
                    int? dexLimiter = armorEntity.DexBonusLimeterAC != null
                        ? (int?)armorEntity.DexBonusLimeterAC.Value
                        : null;

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

                    int dexMax = Mathf.Clamp(dexLimiter ?? CombatOverhaul.Combat.Calculators.ArmorCalculator.GuessDexMaxByArmorGroup(armorEntity), 0, 8);
                    percent = CombatOverhaul.Combat.Calculators.ArmorCalculator.ComputeUniversalAcReductionPercent(dexMax);
                }
                else
                {
                    // 2) Sin armadura: feats marcadores (refs canónicas)
                    var desc = unit.Descriptor;
                    if (desc != null)
                    {
                        var heavyRef = CombatOverhaul.Utils.MarkerRefs.HeavyRef;
                        var mediumRef = CombatOverhaul.Utils.MarkerRefs.MediumRef;

                        if (heavyRef != null && desc.HasFact(heavyRef))
                            percent = 24;
                        else if (mediumRef != null && desc.HasFact(mediumRef))
                            percent = 12;
                    }
                }

                if (percent <= 0) return;

                // Aplicar reducción
                static int Reduce(int v, int pct)
                {
                    if (v <= 0) return v;
                    int r = (v * pct + 99) / 100; // ceil
                    if (r <= 0) r = 1;
                    int res = v - r;
                    return res < 0 ? 0 : res;
                }

                __instance.Touch = Reduce(__instance.Touch, percent);
                __instance.FlatFootedTouch = Reduce(__instance.FlatFootedTouch, percent);
                __instance.FlatFooted = Reduce(__instance.FlatFooted, percent);
                __instance.ModifiedValue = Reduce(__instance.ModifiedValue, percent);
            }
            catch
            {
                // silencioso
            }
        }
    }
}
