using CombatOverhaul.Bus;
using Kingmaker;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;

namespace CombatOverhaul
{
    internal static class Bootstrap
    {
        private static bool _subscribed;
        private static ForceDexForAttack _handler;
        
        // Recalcula los stats de armadura/escudo ya equipados para reflejar ArmorBonus=0
        private static bool _armorRecalcDone;
        
        // evita doble recalculo
        private static bool _recalcDone;

        internal static void Init()
        {
            if (_subscribed) return;

            _handler = new ForceDexForAttack();
            EventBus.Subscribe(_handler);
            _subscribed = true;

            // Recalibra una vez piezas ya equipadas (armadura/escudo) para quitar limitadores previos.
            RecalcMaxDexAllUnitsOnce();
            RecalcAllArmorOnce();
            //RangedFeatFamily.Configure();
        }

        internal static void Dispose()
        {
            if (!_subscribed) return;
            EventBus.Unsubscribe(_handler);
            _handler = null;
            _subscribed = false;
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
                    if (armor != null)
                        armor.RecalculateMaxDexBonus();

                    var shieldArmor = u?.Body?.SecondaryHand?.MaybeShield?.ArmorComponent;
                    if (shieldArmor != null)
                        shieldArmor.RecalculateMaxDexBonus();
                }
            }
            catch { /* no romper nada si aún no hay estado */ }
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
            catch { /* silencioso */ }
        }
    }
}
