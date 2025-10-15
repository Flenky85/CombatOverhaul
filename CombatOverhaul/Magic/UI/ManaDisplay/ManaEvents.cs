using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatOverhaul.Magic.UI.ManaDisplay
{
    internal interface IManaChangedHandler : IUnitSubscriber
    {
        void OnManaChanged(int current, int max);
    }

    internal static class ManaEvents
    {
        private static readonly Dictionary<UnitEntityData, HashSet<Action<int, int>>> _subs =
            new Dictionary<UnitEntityData, HashSet<Action<int, int>>>();

        private static readonly Dictionary<UnitEntityData, UnitBridge> _bridges =
            new Dictionary<UnitEntityData, UnitBridge>();

        public static void Subscribe(UnitEntityData unit, Action<int, int> cb)
        {
            if (unit == null || cb == null) return;

            if (!_subs.TryGetValue(unit, out var set))
            {
                set = new HashSet<Action<int, int>>();
                _subs[unit] = set;
            }

            set.Add(cb);

            EnsureBridge(unit);
        }

        public static void Unsubscribe(UnitEntityData unit, object owner)
        {
            if (unit == null) return;

            if (_subs.TryGetValue(unit, out var set))
            {
                var snap = set.ToArray();
                for (int i = 0; i < snap.Length; i++)
                {
                    var a = snap[i];
                    if (a == null || a.Target == owner)
                        set.Remove(a);
                }

                if (set.Count == 0)
                {
                    _subs.Remove(unit);
                    RemoveBridge(unit);
                }
            }
        }

        public static void Raise(UnitEntityData unit, int current, int max)
        {
            if (unit == null) return;

            if (_bridges.ContainsKey(unit))
            {
                Kingmaker.PubSubSystem.EventBus
                    .RaiseEvent<IManaChangedHandler>(unit, h => h.OnManaChanged(current, max));
                return;
            }

            NotifyLocal(unit, current, max);
        }

        private static void NotifyLocal(UnitEntityData unit, int current, int max)
        {
            if (unit == null) return;
            if (_subs.TryGetValue(unit, out var set))
            {
                var snap = set.ToArray();

                for (int i = 0; i < snap.Length; i++)
                {
                    try { snap[i]?.Invoke(current, max); }
                    catch (Exception) { }
                }

                bool removedAny = false;
                for (int i = 0; i < snap.Length; i++)
                {
                    var a = snap[i];
                    if (a == null)
                    {
                        removedAny = set.Remove(a) || removedAny;
                        continue;
                    }

                    var tgt = a.Target as UnityEngine.Object;
                    if (tgt == null)
                    {
                        removedAny = set.Remove(a) || removedAny;
                    }
                }

                if (removedAny && set.Count == 0)
                {
                    _subs.Remove(unit);
                    RemoveBridge(unit);
                }
            }
        }

        private static void EnsureBridge(UnitEntityData unit)
        {
            if (unit == null) return;
            if (_bridges.ContainsKey(unit)) return;

            var bridge = new UnitBridge(unit);
            Kingmaker.PubSubSystem.EventBus.Subscribe(bridge);
            _bridges[unit] = bridge;
        }

        private static void RemoveBridge(UnitEntityData unit)
        {
            if (unit == null) return;
            if (_bridges.TryGetValue(unit, out var bridge))
            {
                Kingmaker.PubSubSystem.EventBus.Unsubscribe(bridge);
                _bridges.Remove(unit);
            }
        }

        private sealed class UnitBridge : IManaChangedHandler
        {
            private readonly UnitEntityData _unit;
            public UnitBridge(UnitEntityData unit) { _unit = unit; }
            public UnitEntityData GetSubscribingUnit() => _unit;

            public void OnManaChanged(int current, int max)
            {
                NotifyLocal(_unit, current, max);
            }
        }
    }

    internal sealed class OnDestroyHook : MonoBehaviour
    {
        public Action OnDestroyed;
        private void OnDestroy() => OnDestroyed?.Invoke();
    }
}
