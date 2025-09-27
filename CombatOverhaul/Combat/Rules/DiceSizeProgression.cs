using System;
using System.Collections.Generic;
using Kingmaker.RuleSystem;

namespace CombatOverhaul.Combat.Rules
{
    /// Progresión oficial PF1 para aumento de “tamaño” del dado de daño.
    internal static class DiceSizeProgression
    {
        // Secuencia genérica
        private static readonly (int r, DiceType d)[] Gen = {
            (0, DiceType.One), // relleno
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

        // Ramas especiales desde dados base “no genéricos”
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

        public static DiceFormula Promote(DiceFormula current, int steps)
        {
            if (steps <= 0) return current;

            var key = (current.Rolls, current.Dice);
            if (Branches.TryGetValue(key, out var chain))
                return PromoteAlong(chain, steps, current);

            int idx = IndexInGen(current);
            if (idx < 0) idx = ApproxIndex(current);
            int target = Math.Min(idx + steps, Gen.Length - 1);
            var t = Gen[target];
            return new DiceFormula(t.r, t.d);
        }

        private static DiceFormula PromoteAlong((int r, DiceType d)[] chain, int steps, DiceFormula start)
        {
            int i = Math.Min(steps, chain.Length);
            var t = chain[i - 1];
            return new DiceFormula(t.r, t.d);
        }

        private static int IndexInGen(DiceFormula f)
        {
            for (int i = 0; i < Gen.Length; i++)
                if (Gen[i].r == f.Rolls && Gen[i].d == f.Dice) return i;
            return -1;
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
                    case DiceType.D10: return 6; // ~ 2d6
                    case DiceType.D12: return 7; // ~ 3d6
                }
            }
            return 5; // por defecto ~1d8
        }
    }
}
