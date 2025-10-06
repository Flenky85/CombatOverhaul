using System;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;

namespace CombatOverhaul.Patches.Movement
{
    [HarmonyPatch(typeof(UnitDescriptor), "Initialize", new Type[] { typeof(UnitDescriptor), typeof(BlueprintUnit) })]
    internal static class ReduceSpeed
    {
        private const double Scale = 0.70; 

        [HarmonyPostfix]
        static void Postfix(UnitDescriptor unit)
        {
            var speed = unit?.Stats?.Speed;
            if (speed == null) return;

            int baseVal = speed.BaseValue;                  
            int scaled = (int)Math.Round(baseVal * Scale, MidpointRounding.AwayFromZero);
            speed.BaseValue = RoundToNearest5(Math.Max(scaled, 5));
        }

        private static int RoundToNearest5(int v)
        {
            int rem = v % 5;
            if (rem == 0) return v;
            return rem >= 3 ? v + (5 - rem) : v - rem;
        }
    }
}
