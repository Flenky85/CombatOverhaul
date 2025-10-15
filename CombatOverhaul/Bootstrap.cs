using Kingmaker;

namespace CombatOverhaul
{
    internal static class Bootstrap
    {
        private static bool _armorRecalcDone;
        private static bool _recalcDone;

        internal static void InitOnce()
        {
            RecalcMaxDexAllUnitsOnce();
            RecalcAllArmorOnce();
        }

        internal static void Reset()
        {
            _armorRecalcDone = false;
            _recalcDone = false;
        }

        private static void RecalcMaxDexAllUnitsOnce()
        {
            if (_recalcDone) return;
            _recalcDone = true;

            try
            {
                var game = Game.Instance;
                if (game?.State?.Units == null) return;

                foreach (var u in game.State.Units)
                {
                    var armor = u?.Body?.Armor?.MaybeArmor;
                    armor?.RecalculateMaxDexBonus();

                    var shieldArmor = u?.Body?.SecondaryHand?.MaybeShield?.ArmorComponent;
                    shieldArmor?.RecalculateMaxDexBonus();
                }
            }
            catch { /* swallow */ }
        }

        private static void RecalcAllArmorOnce()
        {
            if (_armorRecalcDone) return;
            _armorRecalcDone = true;

            try
            {
                var units = Game.Instance?.State?.Units;
                if (units == null) return;

                foreach (var u in units)
                {
                    var armor = u?.Body?.Armor?.MaybeArmor;
                    armor?.RecalculateStats();

                    var shieldArmor = u?.Body?.SecondaryHand?.MaybeShield?.ArmorComponent;
                    shieldArmor?.RecalculateStats();
                }
            }
            catch { /* swallow */ }
        }
    }
}
