using Kingmaker.PubSubSystem;
using CombatOverhaul.Handlers;

namespace CombatOverhaul
{
    internal static class Bootstrap
    {
        private static bool _subscribed;
        private static ForceDexForAttack _handler;

        internal static void Init()
        {
            if (_subscribed) return;
            _handler = new ForceDexForAttack();
            EventBus.Subscribe(_handler);  
            _subscribed = true;
        }

        internal static void Dispose()
        {
            if (!_subscribed) return;
            EventBus.Unsubscribe(_handler);
            _handler = null;
            _subscribed = false;
        }
    }
}
