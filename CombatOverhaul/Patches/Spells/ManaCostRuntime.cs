using CombatOverhaul.Resources;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Linq;
using UnityEngine;

namespace CombatOverhaul.Patches.Spells
{
    [HarmonyPatch]
    internal static class ManaCostRuntime_Level1
    {
        private const int Level1Cost = 10;

        // ========= Helpers =========
        private static bool IsPartyInCombatCaster(AbilityData ability)
        {
            var caster = ability?.Caster?.Unit;
            return caster != null && caster.IsPlayerFaction && caster.IsInCombat && Game.Instance?.Player?.IsInCombat == true;
        }

        private static bool IsLevel1Spell(AbilityData ad)
        {
            var ab = ad?.Blueprint;
            if (ab == null) return false;

            // Evita variantes de toque (para no cobrar dos veces)
            bool isTouchVariant =
                ab.GetComponent<AbilityEffectStickyTouch>() != null ||
                ab.GetComponent<AbilityDeliverTouch>() != null;

            if (isTouchVariant) return false;

            // Usa el nivel “runtime” que ya calcula AbilityData (sirve para listas/clases/items)
            // 0 = cantrip, 1 = lo que buscamos
            return ad.SpellLevel == 1 && ab.IsSpell;
        }

        private static bool HasEnoughMana(UnitEntityData unit, int cost)
        {
            var res = ManaResourceBP.Mana;
            var coll = unit?.Descriptor?.Resources;
            if (res == null || coll == null) return false;
            if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);
            return coll.GetResourceAmount(res) >= cost;
        }

        private static void SpendMana(UnitEntityData unit, int cost)
        {
            var res = ManaResourceBP.Mana;
            var coll = unit?.Descriptor?.Resources;
            if (res == null || coll == null) return;
            if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);

            int max = CombatOverhaul.Calculators.ManaCalc.CalcMaxMana(unit);
            int cur = coll.GetResourceAmount(res);
            int after = Mathf.Clamp(cur - cost, 0, max);

            // setter interno (tu build no tiene SetResourceAmount público)
            var map = coll.m_Resources;
            if (map != null && map.TryGetValue(res, out var uar) && uar != null)
                uar.Amount = after;

            // refresca UI
            UI.Mana.ManaEvents.Raise(unit, after, max);
        }

        // ============================================================
        // 1) BLOQUEAR el casteo si no hay maná: reduce AvailableCount a 0
        //    AbilityData.GetAvailableForCastCount() existe en tu build.
        // ============================================================
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.GetAvailableForCastCount))]
        private static class AbilityData_GetAvailableForCastCount_Patch
        {
            static void Postfix(AbilityData __instance, ref int __result)
            {
                try
                {
                    if (__result <= 0) return; // ya está bloqueado por el juego vanilla
                    if (!IsPartyInCombatCaster(__instance)) return;
                    if (!IsLevel1Spell(__instance)) return;

                    var caster = __instance.Caster?.Unit;
                    if (caster == null) return;

                    if (!HasEnoughMana(caster, Level1Cost))
                        __result = 0; // sin maná -> no se puede castear
                }
                catch { /* swallow */ }
            }
        }

        // ================================================
        // 2) GASTAR maná al lanzar: AbilityData.Spend()
        // ================================================
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.Spend))]
        private static class AbilityData_Spend_Patch
        {
            static void Postfix(AbilityData __instance)
            {
                try
                {
                    if (!IsPartyInCombatCaster(__instance)) return;
                    if (!IsLevel1Spell(__instance)) return;

                    var caster = __instance.Caster?.Unit;
                    if (caster == null) return;

                    // === Guardias para NO gastar si no hay suficiente maná ===
                    var res = ManaResourceBP.Mana;
                    var coll = caster.Descriptor?.Resources;
                    if (res == null || coll == null) return;
                    if (!coll.ContainsResource(res)) coll.Add(res, restoreAmount: false);

                    int cur = coll.GetResourceAmount(res);
                    if (cur < Level1Cost) return; // <-- CLAVE: no gastes si no llega

                    // (Opcional) doble check: si el juego considera que no está disponible, no gastar
                    // if (__instance.GetAvailableForCastCount() <= 0) return;

                    int max = Calculators.ManaCalc.CalcMaxMana(caster);
                    int after = Mathf.Clamp(cur - Level1Cost, 0, max);

                    var map = coll.m_Resources; // setter interno: tu build no tiene SetResourceAmount público
                    if (map != null && map.TryGetValue(res, out var uar) && uar != null)
                        uar.Amount = after;

                    UI.Mana.ManaEvents.Raise(caster, after, max);
                }
                catch { /* swallow */ }
            }
        }

    }
}
