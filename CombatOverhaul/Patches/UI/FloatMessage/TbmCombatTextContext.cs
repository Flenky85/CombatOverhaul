namespace CombatOverhaul.Patches.UI.FloatMessage
{
    internal static class TbmCombatTextContext
    {
        public static int? OverrideTN { get; private set; }
        public static bool Suppress { get; private set; }

        public static void Set(int tn) { OverrideTN = tn; Suppress = false; }
        public static void Clear() { OverrideTN = null; Suppress = false; }
    }
}
