using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.PubSubSystem;
using System;
using System.Collections.Generic;
using UnityModManagerNet;

namespace CombatOverhaul
{
    internal static class Main
    {
        private const string HarmonyId = "com.combatoverhaul.core";

        private static Harmony _harmony;
        private static UnityModManager.ModEntry _mod;
        private static bool _enabled;

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

                    SubscribeHandlers();

                    Bootstrap.InitOnce();

                    Log.Info("Enabled. Harmony patches applied, handlers subscribed and one-shot recalcs done.");
                }
                else
                {
                    UnsubscribeHandlers();

                    _harmony?.UnpatchAll(HarmonyId);
                    _harmony = null;

                    Bootstrap.Reset();

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
                UnsubscribeHandlers();

                _harmony?.UnpatchAll(HarmonyId);
                _harmony = null;

                _enabled = false;

                Bootstrap.Reset();

                Log.Info("Unloaded.");
            }
            catch (Exception ex)
            {
                Log.Error("Error on unload.", ex);
            }
            return true;
        }

        private static void SubscribeHandlers()
        {
            _busSubs.Clear();

            TrySub(() => EventBus.Subscribe(new Attack.EventBus.ForceDexForAttack()));
            TrySub(() => EventBus.Subscribe(new Magic.EventBus.IntelligenceMagicDamageScaling()));
            TrySub(() => EventBus.Subscribe(new Magic.EventBus.IntelligenceHealingScaling()));
            TrySub(() => EventBus.Subscribe(new Damage.EventBus.BABDice_WeaponStats()));
            TrySub(() => EventBus.Subscribe(new Damage.EventBus.AttackDamageScaling()));
            TrySub(() => EventBus.Subscribe(new CombatState.NewRoundSweep()));

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
