using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityModManagerNet;
using Kingmaker.PubSubSystem; // EventBus
using CombatOverhaul.Utils;

namespace CombatOverhaul
{
    internal static class Main
    {
        private const string HarmonyId = "com.combatoverhaul.core";

        private static Harmony _harmony;
        private static UnityModManager.ModEntry _mod;
        private static bool _enabled;

        // Guardamos los IDisposable devueltos por EventBus.Subscribe
        private static readonly List<IDisposable> _busSubs = new List<IDisposable>();

        // ---------- UMM entry ----------
        static bool Load(UnityModManager.ModEntry entry)
        {
            _mod = entry;
            entry.OnToggle = OnToggle;
            entry.OnUnload = OnUnload;
            Log.Info("UMM entry loaded.");
            return true;
        }

        // ---------- Toggle ----------
        private static bool OnToggle(UnityModManager.ModEntry entry, bool value)
        {
            if (_enabled == value) return true;

            try
            {
                _enabled = value;
                if (value)
                {
                    // 1) Aplicar parches
                    _harmony = new Harmony(HarmonyId);
                    _harmony.PatchAll(typeof(Main).Assembly);

                    // 2) Suscribirse a EventBus
                    SubscribeHandlers();

                    Log.Info("Enabled. Harmony patches applied and handlers subscribed.");
                }
                else
                {
                    // 1) Desuscribir handlers
                    UnsubscribeHandlers();

                    // 2) Retirar parches
                    if (_harmony != null) _harmony.UnpatchAll(HarmonyId);
                    _harmony = null;

                    Log.Info("Disabled. Handlers unsubscribed and patches removed.");
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Toggle failed.", ex);
                return false;
            }
        }

        // ---------- Unload ----------
        private static bool OnUnload(UnityModManager.ModEntry entry)
        {
            try
            {
                UnsubscribeHandlers();

                if (_harmony != null) _harmony.UnpatchAll(HarmonyId);
                _harmony = null;

                _enabled = false;
                Log.Info("Unloaded.");
            }
            catch (Exception ex)
            {
                Log.Error("Error on unload.", ex);
            }
            return true;
        }

        // ---------- Suscripciones ----------
        private static void SubscribeHandlers()
        {
            _busSubs.Clear();

            TrySub(() => EventBus.Subscribe(new CombatOverhaul.Patches.Attack.ForceDexForAttack()));
            TrySub(() => EventBus.Subscribe(new CombatOverhaul.Combat.Rules.Bus.IntelligenceMagicDamageScaling()));
            TrySub(() => EventBus.Subscribe(new CombatOverhaul.Combat.Rules.Bus.IntelligenceHealingScaling()));
            TrySub(() => EventBus.Subscribe(new CombatOverhaul.Combat.Rules.Bus.BABDice_WeaponStats()));
            TrySub(() => EventBus.Subscribe(new CombatOverhaul.Combat.Rules.Bus.StrengthPercentPerPoint()));

            Log.Info($"Subscribed {_busSubs.Count} handlers.");
        }

        private static void TrySub(Func<IDisposable> subscribeFn)
        {
            try
            {
                var d = subscribeFn();
                if (d != null) _busSubs.Add(d);
            }
            catch (Exception ex)
            {
                Log.Error("Subscribe failed.", ex);
            }
        }

        private static void UnsubscribeHandlers()
        {
            foreach (var d in _busSubs)
            {
                try { d?.Dispose(); }
                catch (Exception ex) { Log.Error("Unsubscribe failed.", ex); }
            }
            _busSubs.Clear();
        }
    }
}
