using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kingmaker.RuleSystem.Rules.Damage;

namespace CombatOverhaul.Rules
{
    internal static class DamageReduction_FactorStore
    {
        private sealed class Entry
        {
            public readonly List<float> Factors = new List<float>(4);
            public int Index;
            public DateTime StampUtc;
        }

        private static readonly ConditionalWeakTable<RuleDealDamage, Entry> Table =
            new ConditionalWeakTable<RuleDealDamage, Entry>();

        private static readonly TimeSpan Expiration = TimeSpan.FromSeconds(5);

        public static void Set(RuleDealDamage rule, List<float> factors)
        {
            if (rule == null || factors == null || factors.Count == 0)
                return;

            var entry = Table.GetOrCreateValue(rule);

            lock (entry)
            {
                if (entry.Factors.Capacity < factors.Count)
                    entry.Factors.Capacity = factors.Count;

                entry.Factors.Clear();
                entry.Factors.AddRange(factors);
                entry.Index = 0;
                entry.StampUtc = DateTime.UtcNow;
            }
        }

        public static bool TryDequeue(RuleDealDamage rule, out float factor)
        {
            factor = 1f;
            if (rule == null) return false;

            if (!Table.TryGetValue(rule, out var entry))
                return false;

            lock (entry)
            {
                if (IsExpired(entry))
                {
                    Table.Remove(rule);
                    return false;
                }

                if (entry.Index >= entry.Factors.Count)
                {
                    Table.Remove(rule);
                    return false;
                }

                factor = entry.Factors[entry.Index++];

                if (entry.Index >= entry.Factors.Count)
                    Table.Remove(rule);

                return true;
            }
        }

        public static bool TryPeek(RuleDealDamage rule, out float factor)
        {
            factor = 1f;
            if (rule == null) return false;

            if (!Table.TryGetValue(rule, out var entry))
                return false;

            lock (entry)
            {
                if (IsExpired(entry))
                {
                    Table.Remove(rule);
                    return false;
                }

                if (entry.Index >= entry.Factors.Count)
                {
                    Table.Remove(rule);
                    return false;
                }

                factor = entry.Factors[entry.Index];
                return true;
            }
        }

        public static void Clear(RuleDealDamage rule)
        {
            if (rule == null) return;
            Table.Remove(rule);
        }

        public static int Remaining(RuleDealDamage rule)
        {
            if (rule == null) return 0;

            if (!Table.TryGetValue(rule, out var entry))
                return 0;

            lock (entry)
            {
                if (IsExpired(entry))
                {
                    Table.Remove(rule);
                    return 0;
                }

                int remaining = entry.Factors.Count - entry.Index;
                if (remaining <= 0)
                {
                    Table.Remove(rule);
                    return 0;
                }

                return remaining;
            }
        }

        private static bool IsExpired(Entry e) =>
            (DateTime.UtcNow - e.StampUtc) > Expiration;
    }
}
