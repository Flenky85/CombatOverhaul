using System;
using System.Collections.Generic;
using Kingmaker.EntitySystem.Entities;
using UnityEngine;

namespace CombatOverhaul.Magic.UI.ManaDisplay
{
    internal static class ManaEvents
    {
        private static readonly Dictionary<UnitEntityData, List<Action<int, int>>> _subs =
            new Dictionary<UnitEntityData, List<Action<int, int>>>();

        public static void Subscribe(UnitEntityData unit, Action<int, int> cb)
        {
            if (unit == null || cb == null) return;
            if (!_subs.TryGetValue(unit, out var list))
            {
                list = new List<Action<int, int>>();
                _subs[unit] = list;
            }
            if (!list.Contains(cb)) list.Add(cb);
        }

        public static void Unsubscribe(UnitEntityData unit, object owner)
        {
            if (unit == null) return;
            if (_subs.TryGetValue(unit, out var list))
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var a = list[i];
                    if (a == null || a.Target == owner) list.RemoveAt(i);
                }
                if (list.Count == 0) _subs.Remove(unit);
            }
        }

        public static void Raise(UnitEntityData unit, int current, int max)
        {
            if (unit == null) return;
            if (_subs.TryGetValue(unit, out var list))
            {
                var copy = list.ToArray();
                for (int i = 0; i < copy.Length; i++)
                    copy[i]?.Invoke(current, max);
            }
        }
    }

    internal sealed class OnDestroyHook : MonoBehaviour
    {
        public Action OnDestroyed;
        private void OnDestroy() => OnDestroyed?.Invoke();
    }
}
