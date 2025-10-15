using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Magic.UI.ManaDisplay
{
    public static class ManaUI
    {
        private sealed class LastPair { public bool Init; public int Cur; public int Max; }
        private static readonly ConditionalWeakTable<UnitEntityData, LastPair> _last =
            new ConditionalWeakTable<UnitEntityData, LastPair>();


        public static void SetManaResource(BlueprintAbilityResource res)
            => ManaProvider.ManaResource = res;

        public static void RefreshUnit(UnitEntityData unit)
        {
            if (unit == null) return;

            var (current, max) = ManaProvider.Get(unit);

            if (!_last.TryGetValue(unit, out var box))
            {
                box = new LastPair();
                _last.Add(unit, box);
            }

            if (!box.Init || box.Cur != current || box.Max != max)
            {
                box.Init = true;
                box.Cur = current;
                box.Max = max;
                ManaEvents.Raise(unit, current, max); 
            }
        }
    }

    internal static class ManaProvider
    {
        public static BlueprintAbilityResource ManaResource;

        public static (int current, int max) Get(UnitEntityData unit)
        {
            if (unit == null || ManaResource == null) return (0, 0);
            var desc = unit.Descriptor;
            if (desc == null) return (0, 0);

            int max = ManaCalc.CalcMaxMana(unit);
            var coll = desc.Resources;
            int cur = coll.GetResourceAmount(ManaResource);

            if (cur > max) cur = max;
            if (cur < 0) cur = 0;
            return (cur, max);
        }
    }
}
