using System;
using System.Collections.Generic;
using CombatOverhaul.CombatState;
using CombatOverhaul.Utils;
using Kingmaker.EntitySystem.Entities;

namespace CombatOverhaul.Damage
{
    internal static class SneakOnceManager
    {
        private static readonly HashSet<string> UsedThisRound = new HashSet<string>(StringComparer.Ordinal);

        static SneakOnceManager()
        {
            NewRoundSweep.OnUnitNewRound += ResetForUnit;
        }

        public static bool HasSpent(UnitEntityData unit)
        {
            if (unit == null) return false;
            return UsedThisRound.Contains(GetKey(unit));
        }

        public static void MarkSpent(UnitEntityData unit)
        {
            if (unit == null) return;
            UsedThisRound.Add(GetKey(unit));
#if DEBUG
            Log.DebugLog("[SneakOnce] MarkSpent " + unit.CharacterName);
#endif
        }

        private static void ResetForUnit(UnitEntityData unit)
        {
            if (unit == null) return;
            UsedThisRound.Remove(GetKey(unit));
#if DEBUG
            Log.DebugLog("[SneakOnce] ResetForUnit " + unit.CharacterName);
#endif
        }

        private static string GetKey(UnitEntityData unit)
        {
            try
            {
                var uidObj = unit.UniqueId;           
                if (!string.IsNullOrEmpty(uidObj))
                    return uidObj;
            }
            catch
            {

            }

            return unit.GetHashCode().ToString();
        }
    }
}
