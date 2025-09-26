using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Kingmaker.RuleSystem.Rules.Damage;

namespace CombatOverhaul.Combat.Rules
{
    internal static class ArmorDR_FactorStore
    {
        private sealed class Entry
        {
            public readonly List<float> Factors = new List<float>();
            public int Index;
            public DateTime Stamp = DateTime.UtcNow;
        }

        private static readonly ConditionalWeakTable<RuleDealDamage, Entry> Table =
            new ConditionalWeakTable<RuleDealDamage, Entry>();

        public static void Set(RuleDealDamage rule, List<float> factors)
        {
            if (rule == null || factors == null || factors.Count == 0) return;

            Entry e;
            if (!Table.TryGetValue(rule, out e))
            {
                e = new Entry();
                Table.Add(rule, e);
            }

            e.Factors.Clear();
            e.Factors.AddRange(factors);
            e.Index = 0;
            e.Stamp = DateTime.UtcNow;
        }

        public static bool TryDequeue(RuleDealDamage rule, out float factor)
        {
            factor = 1f;
            if (rule == null) return false;

            Entry e;
            if (!Table.TryGetValue(rule, out e)) return false;

            // caduca por si acaso (anti fugas si algo quedara colgado)
            if ((DateTime.UtcNow - e.Stamp).TotalSeconds > 5.0)
                return false;

            if (e.Index >= e.Factors.Count) return false;

            factor = e.Factors[e.Index++];
            return true;
        }
    }
}
