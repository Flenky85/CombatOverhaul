// CombatOverhaul/UI/TbmCombatTextContext.cs
using System;

namespace CombatOverhaul.UI
{
    internal static class TbmCombatTextContext
    {
        [ThreadStatic] public static int? OverrideTN;
        [ThreadStatic] public static int? OverridePct;

        public static void Set(int tn, int pct) { OverrideTN = tn; OverridePct = pct; }
        public static void Clear() { OverrideTN = null; OverridePct = null; }
    }
}
