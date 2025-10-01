using System.Runtime.CompilerServices;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Combat.Opposed
{
    internal static class OpposedRollStore
    {
        private static readonly ConditionalWeakTable<RuleAttackRoll, OpposedRollCore.Result> _map
            = new ConditionalWeakTable<RuleAttackRoll, OpposedRollCore.Result>();

        internal static void Save(RuleAttackRoll roll, OpposedRollCore.Result res)
        {
            if (roll == null) return;
            _map.Remove(roll);
            _map.Add(roll, res);
        }

        internal static bool TryGet(RuleAttackRoll roll, out OpposedRollCore.Result res)
            => _map.TryGetValue(roll, out res);
    }
}
