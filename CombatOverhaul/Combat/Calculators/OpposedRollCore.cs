using System;
using CombatOverhaul.Utils; // Log opcional

namespace CombatOverhaul.Combat.Calculators
{
    internal static class OpposedRollCore
    {
        internal static float Alpha = 1.3f;   // slope
        internal static float Beta = 0.09f;   // attacker bias
        internal static float Floor = 0.05f;  // 5%
        internal static float Ceil = 0.95f;   // 95%
        internal static float Step = 0.05f;   // 5% step size (quantization)

        // Debug toggle (you can hook this to your config)
        internal static bool EnableDebugLog = false;

        private const float EPS = 0.0001f;

        internal sealed class Result
        {
            public float A { get; set; }
            public float D { get; set; }
            public float BaseP { get; set; }  
            public float PAdj { get; set; }   
            public float P5 { get; set; }     
            public int TN { get; set; }       
            public int D20 { get; set; }      
            public bool Success { get; set; } 
        }

        internal static Result ResolveD20(int attackBonus, int targetAC, int d20)
        {
            float A = Math.Max(0, attackBonus);
            float D = Math.Max(0, targetAC);

            float baseP = A + D <= EPS ? 0.5f : A / (A + D);

            float pAdj = Clamp(baseP * Alpha + Beta, Floor, Ceil);
            float p5 = RoundToStep(pAdj, Step);

            int tn = Clamp(21 - (int)Math.Round(p5 * 20f), 2, 20);
            bool success = d20 >= tn;

            var res = new Result
            {
                A = A,
                D = D,
                BaseP = baseP,
                PAdj = pAdj,
                P5 = p5,
                TN = tn,
                D20 = d20,
                Success = success
            };

            if (EnableDebugLog)
            {
                Log.Info($"[Opposed] ATK A={A:0.##} D={D:0.##} | baseP={baseP:P0}  α={Alpha:0.##} β={Beta:0.##} " +
                         $"→ pAdj={pAdj:P0} → p5={p5:P0} → TN={tn} | d20={d20} ⇒ {(success ? "HIT" : "MISS")}");
            }

            return res;
        }


        private static int Clamp(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        private static float Clamp(float v, float min, float max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        private static float RoundToStep(float value, float step)
        {
            if (step <= 0f) return value; 
            return (float)Math.Round(value / step) * step;
        }
    }
}
