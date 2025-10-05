using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kingmaker.RuleSystem.Rules.Damage;

namespace CombatOverhaul.Rules
{
    internal static class ArmorDR_FactorStore
    {
        private sealed class Entry
        {
            public readonly List<float> Factors = new List<float>();
            public int Index;
            public DateTime Stamp;
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
                entry.Factors.Clear();
                entry.Factors.AddRange(factors);
                entry.Index = 0;
                entry.Stamp = DateTime.UtcNow;
            }
        }

        public static bool TryDequeue(RuleDealDamage rule, out float factor)
        {
            factor = 1f;
            if (rule == null)
                return false;

            if (!Table.TryGetValue(rule, out var entry))
                return false;

            lock (entry)
            {
                var now = DateTime.UtcNow;

                if (now - entry.Stamp > Expiration)
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
                {
                    Table.Remove(rule);
                }

                return true;
            }
        }

    }
}
