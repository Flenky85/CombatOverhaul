using CombatOverhaul.Utils;              // MarkerRefs
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;

namespace CombatOverhaul.Patches
{
    // Parchea: UnitDescriptor.Initialize(UnitDescriptor, BlueprintUnit)
    [HarmonyPatch(typeof(UnitDescriptor), "Initialize", new[] { typeof(UnitDescriptor), typeof(BlueprintUnit) })]
    internal static class Patch_MonsterArmorOnInitialize
    {
        static void Postfix(UnitDescriptor unit, BlueprintUnit blueprint)
        {
            if (unit == null) return;

            // No marcar party/aliados del jugador
            if (unit.IsPlayerFaction) return;

            // Si lleva armadura real, no marcamos
            if (unit.Body?.Armor?.HasArmor == true) return;

            // Base stats
            int str = unit.Stats.Strength.BaseValue;
            int dex = unit.Stats.Dexterity.BaseValue;
            int con = unit.Stats.Constitution.BaseValue;

            var heavyRef = MarkerRefs.HeavyRef;   // refs canónicas
            var mediumRef = MarkerRefs.MediumRef;
            if (heavyRef?.Get() == null || mediumRef?.Get() == null) return;

            if (str > dex)
            {
                if (con > dex)
                {
                    if (!unit.HasFact(heavyRef))
                        unit.AddFact(heavyRef);
                }
                else
                {
                    if (!unit.HasFact(mediumRef))
                        unit.AddFact(mediumRef);
                }
            }
        }
    }
}
