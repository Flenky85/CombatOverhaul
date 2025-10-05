using HarmonyLib;
using Kingmaker.Blueprints;      
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;       

namespace CombatOverhaul.Patches.Armor
{
    [HarmonyPatch(typeof(UnitDescriptor), "Initialize", new[] { typeof(UnitDescriptor), typeof(BlueprintUnit) })]
    internal static class MonsterArmorMark
    {
        private static void Postfix(UnitDescriptor unit)
        {
            if (unit == null) return;
            if (unit.IsPlayerFaction) return;
            if (unit.Body?.Armor?.HasArmor == true) return;

            var heavyRef = CombatOverhaul.Utils.MarkerRefs.HeavyRef;
            var mediumRef = CombatOverhaul.Utils.MarkerRefs.MediumRef;
            if (heavyRef?.Get() == null || mediumRef?.Get() == null) return;

            int str = unit.Stats?.Strength?.BaseValue ?? 0;
            int dex = unit.Stats?.Dexterity?.BaseValue ?? 0;
            int con = unit.Stats?.Constitution?.BaseValue ?? 0;

            if (str > dex)
            {
                if (con > dex)
                {
                    if (!Has(unit, heavyRef))
                        unit.AddFact(heavyRef);
                }
                else
                {
                    if (!Has(unit, mediumRef))
                        unit.AddFact(mediumRef);
                }
            }
        }

        private static bool Has(UnitDescriptor d, BlueprintUnitFact fact)
        {
            if (d == null || fact == null) return false;
            var list = d.Facts?.List;
            if (list == null) return false;
            foreach (var f in list)
                if (f != null && ReferenceEquals(f.Blueprint, fact))
                    return true;
            return false;
        }

        private static bool Has(UnitDescriptor d, BlueprintUnitFactReference reference)
            => reference != null && Has(d, reference.Get());
    }
}
