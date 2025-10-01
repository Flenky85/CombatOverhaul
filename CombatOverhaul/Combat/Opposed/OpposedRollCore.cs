using System;
using CombatOverhaul.Utils; // tu Log

namespace CombatOverhaul.Combat.Opposed
{
    internal static class OpposedRollCore
    {
        // === Parámetros por defecto para ATAQUES (puedes exponerlos a config si quieres) ===
        internal static float Alpha = 1.3f;   // pendiente
        internal static float Beta = 0.09f;  // sesgo atacante
        internal static float Floor = 0.05f;  // 5 %
        internal static float Ceil = 0.95f;  // 95 %
        internal static float Step = 0.05f;  // granulado a 5 %

        // Toggle de depuración (actívalo/descártalo desde tu config si quieres)
        internal static bool EnableDebugLog = true;

        internal sealed class Result
        {
            public float A, D;
            public float baseP;    // A/(A+D)
            public float pAdj;     // tras Alpha/Beta y topes
            public float p5;       // redondeado a Step
            public int TN;       // 21 - p5*20
            public int d20;      // tirada
            public bool success;  // d20 >= TN
        }

        /// Calcula TN y devuelve el paquete completo para log.
        internal static Result ResolveForAttack(int attackBonus, int targetAC, int d20)
        {
            float A = Math.Max(0, attackBonus);
            float D = Math.Max(0, targetAC);

            float baseP;
            if (A + D <= 0.0001f) baseP = 0.5f;
            else baseP = A / (A + D);

            float pAdj = baseP * Alpha + Beta;
            pAdj = Math.Max(Floor, Math.Min(Ceil, pAdj));

            float steps = (float)Math.Round(pAdj / Step);
            float p5 = steps * Step;

            int tn = 21 - (int)Math.Round(p5 * 20.0f);
            tn = Math.Max(2, Math.Min(20, tn)); // 2..20 para respetar 1/20 naturales

            bool success = d20 >= tn;

            var res = new Result
            {
                A = A,
                D = D,
                baseP = baseP,
                pAdj = pAdj,
                p5 = p5,
                TN = tn,
                d20 = d20,
                success = success
            };

            if (EnableDebugLog)
            {
                Log.Info(
                    $"[Opposed] ATK A={A:0.##} D={D:0.##} | baseP={baseP:P0}  α={Alpha:0.##} β={Beta:0.##} " +
                    $"→ pAdj={pAdj:P0} → p5={p5:P0} → TN={tn} | d20={d20} ⇒ {(success ? "HIT" : "MISS")}"
                );
            }

            return res;
        }
    }
}
