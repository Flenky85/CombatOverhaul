using CombatOverhaul.Calculators;
using CombatOverhaul.Patches.UI.Mana;
using CombatOverhaul.Resources;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using TurnBased.Controllers;
using UnityEngine;

namespace CombatOverhaul.Patches.Combat
{
    internal static class ManaCombatTestConfig
    {
        public static bool UseFunctionValues = true;
        public static int FixedMaxForTests = 100;
        public static int FixedStartForTests = 40;

        public static int StartFromFunctions = 0;
        public static int BaselineIfZeroMax = 0;
        public static bool ClampStartToMax = true;
    }

    [HarmonyPatch(typeof(CombatController), "HandleCombatStart")]
    internal static class InitManaOnCombatStart
    {
        static void Postfix()
        {
            try
            {
                var game = Game.Instance;
                if (game == null || game.Player == null)
                {                
                    return;
                }

                var party = game.Player.PartyAndPets;
                if (party == null)
                {
                    return;
                }

                var res = ManaResourceBP.Mana;
                if (res == null)
                {
                    return;
                }

                int processed = 0;

                for (int i = 0; i < party.Count; i++)
                {
                    var unit = party[i];
                    if (!IsEligiblePlayerInCombat(unit)) continue;

                    try
                    {
                        int maxDyn, startCur;

                        if (ManaCombatTestConfig.UseFunctionValues)
                        {
                            maxDyn = ManaCalc.CalcMaxMana(unit);
                            if (maxDyn <= 0 && ManaCombatTestConfig.BaselineIfZeroMax > 0)
                            {
                                Debug.Log($"[CO][Mana] '{unit.CharacterName}': CalcMax=0 → using baseline={ManaCombatTestConfig.BaselineIfZeroMax} (tests).");
                                maxDyn = ManaCombatTestConfig.BaselineIfZeroMax;
                            }

                            startCur = maxDyn; // ⟵ START FULL cuando usamos función
                        }
                        else
                        {
                            maxDyn = ManaCombatTestConfig.FixedMaxForTests;
                            startCur = maxDyn; // ⟵ START FULL también en modo fijo
                            // si prefieres respetar FixedStartForTests, cambia esta línea por:
                            // startCur = ManaCombatTestConfig.FixedStartForTests;
                        }

                        if (ManaCombatTestConfig.ClampStartToMax && maxDyn > 0 && startCur > maxDyn)
                            startCur = maxDyn;

                        var coll = unit.Descriptor.Resources;
                        if (!coll.ContainsResource(res))
                            coll.Add(res, restoreAmount: false);

                        int curBefore = coll.GetResourceAmount(res);
                        if (curBefore > 0) coll.Spend(res, curBefore); // limpiar para logs consistentes

                        if (startCur > 0)
                            SetResourceAmountUnsafe(coll, res, startCur); // ⟵ fijamos actual = maxDyn

                        int curAfter = coll.GetResourceAmount(res);
                        

                        ManaEvents.Raise(unit, curAfter, maxDyn); // ⟵ UI con maxDyn


                        Debug.Log($"[CO][Mana] Init '{unit.CharacterName}': curBefore={curBefore} -> curAfter={curAfter}, maxDyn={maxDyn} (FULL)");
                        processed++;
                    }
                    catch (Exception exUnit)
                    {
                        Debug.LogError($"[CO][Mana] Error initializing unit '{unit?.CharacterName ?? "NULL"}': {exUnit}");
                    }
                }

                Debug.Log($"[CO][Mana] HandleCombatStart: Done. Units processed={processed}.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] HandleCombatStart: EX {ex}");
            }
        }

        private static bool IsEligiblePlayerInCombat(UnitEntityData unit)
        {
            if (unit == null) return false;
            if (!unit.IsInGame) return false;
            if (!unit.IsInCombat) return false;
            if (!unit.IsPlayerFaction) return false;
            return true;
        }

        private static void SetResourceAmountUnsafe(UnitAbilityResourceCollection coll, BlueprintScriptableObject res, int value)
        {
            try
            {
                if (coll == null || res == null) return;

                if (!coll.ContainsResource(res))
                    coll.Add(res, restoreAmount: false);

                Dictionary<BlueprintScriptableObject, UnitAbilityResource> map = coll.m_Resources;
                if (!map.TryGetValue(res, out UnitAbilityResource uar) || uar == null)
                    return;

                int old = uar.Amount;
                uar.Amount = Math.Max(0, value);

                // Opcional: notificar cambio nativo
                // EventBus.RaiseEvent<IUnitAbilityResourceHandler>(h => h.HandleAbilityResourceChange(coll.m_Owner, uar, old), true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Mana] SetResourceAmountUnsafe EX: {ex}");
            }
        }
    }
}
