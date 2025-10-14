using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;

namespace CombatOverhaul.Attack.EventBus
{
    internal sealed class ForceDexForAttack :
        IGlobalRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
        IGlobalRulebookHandler<RuleCalculateAttackBonus>,
        ISubscriber, IGlobalSubscriber
    {
        private static readonly Dictionary<object, IDisposable> _scopes =
            new Dictionary<object, IDisposable>(capacity: 32);

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt) => BeginScope(evt);
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt) => EndScope(evt);

        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt) => BeginScope(evt);
        public void OnEventDidTrigger(RuleCalculateAttackBonus evt) => EndScope(evt);


        private static void BeginScope(object evtKey)
        {
            if (evtKey == null) return;

            if (_scopes.TryGetValue(evtKey, out var oldScope))
            {
                try { oldScope.Dispose(); } catch {  }
                _scopes.Remove(evtKey);
            }

            var scope = ContextData<AttackBonusStatReplacement>.Request();
            scope.Stat = StatType.Dexterity; 
            _scopes[evtKey] = scope;
        }

        private static void EndScope(object evtKey)
        {
            if (evtKey == null) return;

            if (_scopes.TryGetValue(evtKey, out var scope))
            {
                _scopes.Remove(evtKey);
                try { scope.Dispose(); } catch {  }
            }
        }
    }
}
