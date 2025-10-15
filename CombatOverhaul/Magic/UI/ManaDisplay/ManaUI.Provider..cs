using CombatOverhaul.Features;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;

namespace CombatOverhaul.Magic.UI.ManaDisplay
{
    public static class ManaUI
    {
        public static void SetManaResource(BlueprintAbilityResource res)
            => ManaProvider.ManaResource = res;

        public static void RefreshUnit(UnitEntityData unit)
        {
            if (unit == null) return;
            var (current, max) = ManaProvider.Get(unit);
            ManaEvents.Raise(unit, current, max);
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
            int cur = coll.ContainsResource(ManaResource) ? coll.GetResourceAmount(ManaResource) : 0;

            if (cur > max) cur = max;
            if (cur < 0) cur = 0;
            return (cur, max);
        }
    }
}
