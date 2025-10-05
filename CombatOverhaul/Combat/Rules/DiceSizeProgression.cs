using System.Collections.Generic;
using Kingmaker.RuleSystem;

namespace CombatOverhaul.Combat.Rules
{
    internal static class DiceSizeProgression
    {
        private static readonly (int r, DiceType d)[] Gen = new (int r, DiceType d)[]
        {
            (0, DiceType.One),
            (1, DiceType.D2),
            (1, DiceType.D3),
            (1, DiceType.D4),
            (1, DiceType.D6),
            (1, DiceType.D8),
            (2, DiceType.D6),
            (3, DiceType.D6),
            (4, DiceType.D6),
            (6, DiceType.D6),
            (8, DiceType.D6),
            (12, DiceType.D6),
        };

        private static readonly Dictionary<(int r, DiceType d), int> GenIndexMap =
            new Dictionary<(int r, DiceType d), int>();

        private static readonly Dictionary<(int r, DiceType d), (int r, DiceType d)[]> Branches =
            new Dictionary<(int, DiceType), (int, DiceType)[]>
            {
                [(1, DiceType.D10)] = new[] { (2, DiceType.D8), (3, DiceType.D8), (4, DiceType.D8), (6, DiceType.D8), (8, DiceType.D8), (12, DiceType.D8) },
                [(1, DiceType.D12)] = new[] { (3, DiceType.D6), (4, DiceType.D6), (6, DiceType.D6), (8, DiceType.D6), (12, DiceType.D6) },
                [(2, DiceType.D4)] = new[] { (2, DiceType.D6), (3, DiceType.D6), (4, DiceType.D6), (6, DiceType.D6), (8, DiceType.D6), (12, DiceType.D6) },
                [(2, DiceType.D6)] = new[] { (3, DiceType.D6), (4, DiceType.D6), (6, DiceType.D6), (8, DiceType.D6), (12, DiceType.D6) },
                [(2, DiceType.D8)] = new[] { (3, DiceType.D8), (4, DiceType.D8), (6, DiceType.D8), (8, DiceType.D8), (12, DiceType.D8) },
                [(3, DiceType.D6)] = new[] { (4, DiceType.D6), (6, DiceType.D6), (8, DiceType.D6), (12, DiceType.D6) },
                [(3, DiceType.D8)] = new[] { (4, DiceType.D8), (6, DiceType.D8), (8, DiceType.D8), (12, DiceType.D8) },
                [(4, DiceType.D6)] = new[] { (6, DiceType.D6), (8, DiceType.D6), (12, DiceType.D6) },
                [(4, DiceType.D8)] = new[] { (6, DiceType.D8), (8, DiceType.D8), (12, DiceType.D8) },
                [(6, DiceType.D6)] = new[] { (8, DiceType.D6), (12, DiceType.D6) },
                [(6, DiceType.D8)] = new[] { (8, DiceType.D8), (12, DiceType.D8) },
                [(8, DiceType.D6)] = new[] { (12, DiceType.D6) },
                [(8, DiceType.D8)] = new[] { (12, DiceType.D8) },
            };

        private const int DefaultApproxIndex = 5; 

        static DiceSizeProgression()
        {
            for (int i = 0; i < Gen.Length; i++)
            {
                GenIndexMap[Gen[i]] = i;
            }
        }

        public static DiceFormula Promote(DiceFormula current, int steps)
        {
            if (steps <= 0) return current;

            var key = (current.Rolls, current.Dice);

            if (Branches.TryGetValue(key, out (int r, DiceType d)[] chain))
            {
                var (r, d) = PromoteAlong(chain, steps, key);
                return new DiceFormula(r, d);
            }

            if (!GenIndexMap.TryGetValue(key, out int idx))
            {
                idx = ApproxIndex(current);
            }

            int target = idx + steps;
            if (target >= Gen.Length) target = Gen.Length - 1;

            var pick = Gen[target];
            return new DiceFormula(pick.r, pick.d);
        }

        private static (int r, DiceType d) PromoteAlong((int r, DiceType d)[] chain, int steps, (int r, DiceType d) currentKey)
        {
            if (steps <= 0) return currentKey;

            int i = steps - 1; 
            if (i >= chain.Length) i = chain.Length - 1;
            return chain[i];
        }

        private static int ApproxIndex(DiceFormula f)
        {
            if (f.Rolls == 1)
            {
                switch (f.Dice)
                {
                    case DiceType.D2: return 1;
                    case DiceType.D3: return 2;
                    case DiceType.D4: return 3;
                    case DiceType.D6: return 4;
                    case DiceType.D8: return 5;
                    case DiceType.D10: return 6;
                    case DiceType.D12: return 7;
                }
            }
            return DefaultApproxIndex;
        }
    }
}
