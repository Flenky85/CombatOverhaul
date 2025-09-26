using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityModManagerNet;
using CombatOverhaul.Utils;
using Kingmaker.PubSubSystem; // EventBus

namespace CombatOverhaul
{
    internal static class Main
    {
        private const string HarmonyId = "com.combatoverhaul.core";
        private static Harmony _harmony;
        private static UnityModManager.ModEntry _mod;
        private static bool _enabled;

        // Mantén una lista de tokens para poder desuscribir
        private static readonly List<IDisposable> _busSubs = new List<IDisposable>();

        static bool Load(UnityModManager.ModEntry entry)
        {
            _mod = entry;
            entry.OnToggle = OnToggle;
            entry.OnUnload = OnUnload;
            Log.Info("UMM entry loaded.");
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry entry, bool value)
        {
            if (_enabled == value) return true;
            try
            {
                _enabled = value;
                if (value)
                {
                    _harmony = new Harmony(HarmonyId);
                    _harmony.PatchAll(typeof(Main).Assembly);

                    // SUSCRIPCIONES AL EVENTBUS
                    _busSubs.Add(EventBus.Subscribe(new CombatOverhaul.Patches.Attack.ForceDexForAttack()));

                    Log.Info("Enabled. Harmony patches applied and handlers subscribed.");
                }
                else
                {
                    // DESUSCRIBIR HANDLERS
                    for (int i = 0; i < _busSubs.Count; i++)
                        if (_busSubs[i] != null) _busSubs[i].Dispose();
                    _busSubs.Clear();

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

        private static bool OnUnload(UnityModManager.ModEntry entry)
        {
            try
            {
                for (int i = 0; i < _busSubs.Count; i++)
                    if (_busSubs[i] != null) _busSubs[i].Dispose();
                _busSubs.Clear();

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
    }
}
