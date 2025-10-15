using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities;

namespace CombatOverhaul.Utils
{
    internal static class PartyUtils
    {
        public static bool IsPartyOrPet(UnitEntityData unit)
        {
            var player = Game.Instance?.Player;
            if (unit == null || player == null) return false;

            var list = player.PartyAndPets;
            if (list == null) return false;

            foreach (var u in list)
                if (ReferenceEquals(u, unit))
                    return true;

            return false;
        }

        public static bool IsPartyOrPetCaster(AbilityData ability)
        {
            var unit = ability?.Caster?.Unit;
            return IsPartyOrPet(unit);
        }

        public static bool IsPartyUnitInCombat(UnitEntityData unit)
        {
            var player = Game.Instance?.Player;
            if (unit == null || player == null) return false;
            if (!player.IsInCombat || !unit.IsInCombat) return false;
            return IsPartyOrPet(unit);
        }

        public static bool IsPartyCasterInCombat(AbilityData ability)
        {
            var unit = ability?.Caster?.Unit;
            return IsPartyUnitInCombat(unit);
        }
    }
}
