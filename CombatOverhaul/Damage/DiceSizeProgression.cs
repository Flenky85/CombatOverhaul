using System.Collections.Generic;
using Kingmaker.RuleSystem;

namespace CombatOverhaul.Damage
{
    internal static class DiceSizeProgression
    {
        private static readonly (int r, DiceType d)[] Gen =
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
            (14, DiceType.D6),
            (16, DiceType.D6),
            (18, DiceType.D6),
            (20, DiceType.D6),
        };

        private static readonly (int r, DiceType d)[] GenD8 =
        {
            (1, DiceType.D8),
            (2, DiceType.D8),
            (3, DiceType.D8),
            (4, DiceType.D8),
            (6, DiceType.D8),
            (8, DiceType.D8),
            (12, DiceType.D8),
            (14, DiceType.D8),
            (16, DiceType.D8),
            (18, DiceType.D8),
            (20, DiceType.D8),
        };

        private static readonly Dictionary<(int r, DiceType d), int> GenIndexMap =
            new Dictionary<(int r, DiceType d), int>(32);

        private static readonly Dictionary<(int r, DiceType d), int> GenD8IndexMap =
            new Dictionary<(int r, DiceType d), int>(16);

        private const int DefaultApproxIndex = 5; 

        static DiceSizeProgression()
        {
            for (int i = 0; i < Gen.Length; i++)
                GenIndexMap[Gen[i]] = i;

            for (int i = 0; i < GenD8.Length; i++)
                GenD8IndexMap[GenD8[i]] = i;
        }

        public static DiceFormula Promote(DiceFormula current, int steps)
        {
            if (steps <= 0) return current;

            var key = (current.Rolls, current.Dice);

            if (GenIndexMap.TryGetValue(key, out int idxGen))
            {
                int target = ClampIndex(idxGen + steps, Gen.Length);
                var (r, d) = Gen[target];
                return new DiceFormula(r, d);
            }
            if (GenD8IndexMap.TryGetValue(key, out int idxD8))
            {
                int target = ClampIndex(idxD8 + steps, GenD8.Length);
                var (r, d) = GenD8[target];
                return new DiceFormula(r, d);
            }

            if (current.Rolls == 1 && current.Dice == DiceType.D10)
            {
                int start = GenD8IndexMap[(2, DiceType.D8)]; 
                int target = ClampIndex(start + steps - 1, GenD8.Length);
                var (r, d) = GenD8[target];
                return new DiceFormula(r, d);
            }
            if (current.Rolls == 1 && current.Dice == DiceType.D12)
            {
                int start = GenIndexMap[(3, DiceType.D6)];
                int target = ClampIndex(start + steps - 1, Gen.Length);
                var (r, d) = Gen[target];
                return new DiceFormula(r, d);
            }
            if (current.Rolls == 2 && current.Dice == DiceType.D4)
            {
                int start = GenIndexMap[(2, DiceType.D6)];
                int target = ClampIndex(start + steps - 1, Gen.Length);
                var (r, d) = Gen[target];
                return new DiceFormula(r, d);
            }

            int approx = ApproxIndex(current);
            int targetFallback = ClampIndex(approx + steps, Gen.Length);
            var res = Gen[targetFallback];
            return new DiceFormula(res.r, res.d);
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

        private static int ClampIndex(int idx, int length)
        {
            if (idx < 0) return 0;
            if (idx >= length) return length - 1;
            return idx;
        }
    }
}
