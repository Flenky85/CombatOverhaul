/*using System;
using System.Collections.Generic;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Handlers
{
    // Anula la aportación de STR al ataque evitando que el método llegue a usar STR.
    // Lo hacemos inyectando AttackBonusStatReplacement=BaseAttackBonus (no es atributo -> bonus=0).
    internal sealed class KillStrengthBranch :
        IGlobalRulebookHandler<RuleCalculateAttackBonusWithoutTarget>,
        IGlobalRulebookHandler<RuleCalculateAttackBonus>,
        ISubscriber, IGlobalSubscriber
    {
        private static readonly Dictionary<object, IDisposable> _scopes = new Dictionary<object, IDisposable>();

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            TryReplaceStat(evt.Initiator, evt.AttackBonusStat, evt.Weapon, evt);
        }
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            DisposeScope(evt);
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            // El with-target delega en m_InnerRule (WithoutTarget), pero por seguridad aplicamos igual
            TryReplaceStat(evt.Initiator, evt.AttackBonusStat, evt.Weapon, evt);
        }
        public void OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
            DisposeScope(evt);
        }

        private static void TryReplaceStat(UnitEntityData unit, StatType attackStat, ItemEntityWeapon weapon, object key)
        {
            if (unit == null) return;

            // Si el método habría usado STR, forzamos un stat "no atributo" para que bonus sea 0
            bool wouldUseStrength =
                (attackStat == StatType.Unknown && weapon != null && weapon.Blueprint != null && !weapon.Blueprint.IsRanged)
                || (attackStat == StatType.Strength);

            if (!wouldUseStrength) return;

            var scope = ContextData<AttackBonusStatReplacement>.Request();
            scope.Stat = StatType.BaseAttackBonus; // GetStat<ModifiableValueAttributeStat>(BAB) => null => bonus=0
            _scopes[key] = scope;
        }

        private static void DisposeScope(object key)
        {
            if (_scopes.TryGetValue(key, out var scope))
            {
                _scopes.Remove(key);
                scope.Dispose();
            }
        }
    }
}
*/