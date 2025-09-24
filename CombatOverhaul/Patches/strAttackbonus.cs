using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;

namespace CombatOverhaul.Handlers
{
    // Fuerza nunca aporta al ataque. Destreza aporta siempre, melee y a distancia.
    internal sealed class ForceDexForAttack :
        IGlobalRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
        IGlobalRulebookHandler<RuleCalculateAttackBonus>,
        ISubscriber, IGlobalSubscriber
    {
        private static readonly Dictionary<object, IDisposable> _scopes =
            new Dictionary<object, IDisposable>();

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            ApplyDexReplacement(evt);
        }
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            DisposeScope(evt);
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            ApplyDexReplacement(evt);
        }
        public void OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
            DisposeScope(evt);
        }

        private static void ApplyDexReplacement(object evtObj)
        {
            // Inyecta un reemplazo de stat a DEX para este cálculo
            var scope = ContextData<AttackBonusStatReplacement>.Request();
            scope.Stat = StatType.Dexterity; // siempre DEX
            _scopes[evtObj] = scope;
        }

        private static void DisposeScope(object evtObj)
        {
            IDisposable scope;
            if (_scopes.TryGetValue(evtObj, out scope))
            {
                _scopes.Remove(evtObj);
                scope.Dispose();
            }
        }
    }
}
