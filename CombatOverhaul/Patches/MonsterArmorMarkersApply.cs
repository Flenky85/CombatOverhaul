/*using CombatOverhaul.Features;
using HarmonyLib;
using Kingmaker;                          // Game.Instance
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;    // UnitEntityData
using Kingmaker.UnitLogic;                // UnitDescriptor
using System.Linq;

namespace CombatOverhaul.Patches
{
    /// Marca una vez en PostLoad. Nunca elimina.
    internal static class MonsterArmorMarkersApply
    {
        // Parche único y estable
        [HarmonyPatch(typeof(UnitDescriptor), "PostLoad")]
        internal static class Patch_PostLoad
        {
            static void Postfix(UnitDescriptor __instance)
            {
                TryApplyOnce(__instance?.Unit);
            }
        }

        // Wrapper público para llamar desde tu init
        public static void TryApplyOnce_Public(UnitEntityData unit) => TryApplyOnce(unit);

        // ===== Helpers =====
        private static UnitEntityData GetMainCharacter()
        {
            var player = Game.Instance?.Player;
            if (player == null) return null;

            var mc = player.MainCharacter.Value;
            if (mc != null) return mc;

            return player.PartyAndPets?.FirstOrDefault(u => u?.Descriptor?.IsMainCharacter ?? false);
        }

        private static bool IsEnemyToPlayer(UnitEntityData unit)
        {
            var mc = GetMainCharacter();
            return mc != null && unit.IsEnemy(mc);
        }

        // ===== Núcleo =====
        private static void TryApplyOnce(UnitEntityData unit)
        {
            if (unit == null) return;
            var desc = unit.Descriptor;
            if (desc == null) return;
            if (!IsEnemyToPlayer(unit)) return;

            // Si ya tiene marcador, salir
            if (desc.HasFact(MonsterArmorMarkers.HeavyRef) || desc.HasFact(MonsterArmorMarkers.MediumRef))
                return;

            // Si lleva armadura real, no marcamos
            if (desc.Body?.Armor?.HasArmor == true)
                return;

            // Base stats
            var str = desc.Stats.Strength.BaseValue;
            var dex = desc.Stats.Dexterity.BaseValue;
            var con = desc.Stats.Constitution.BaseValue;

            if (str > dex)
            {
                if (con > dex)
                    AddMarker(desc, MonsterArmorMarkers.HeavyRef);   // 40% DR, 24% AC pen
                else
                    AddMarker(desc, MonsterArmorMarkers.MediumRef);  // 20% DR, 12% AC pen
            }
        }

        private static void AddMarker(UnitDescriptor desc, BlueprintFeatureReference featRef)
        {
            if (featRef?.Get() == null) return;
            if (desc.HasFact(featRef)) return;
            desc.AddFact(featRef);
        }
    }
}
*/