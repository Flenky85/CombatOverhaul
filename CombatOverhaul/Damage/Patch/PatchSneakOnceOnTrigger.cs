using System;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Damage.Patch
{
    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.OnTrigger))]
    internal static class PatchSneakOnceOnTrigger
    {
        static void Postfix(RuleAttackRoll __instance)
        {
            try
            {
                var attacker = __instance?.Initiator;
                if (attacker == null) return;
                if (!__instance.IsHit) return;
                if (!__instance.IsSneakAttack) return;

                if (SneakOnceManager.HasSpent(attacker))
                {
                    __instance.IsSneakAttack = false;
#if DEBUG
                    Log.DebugLog("[SneakOnce] Blocked (already spent this round). " +
                                 "Attacker=" + attacker.CharacterName + " Target=" + (__instance.Target?.CharacterName ?? "null"));
#endif
                    return;
                }

                SneakOnceManager.MarkSpent(attacker);
#if DEBUG
                Log.DebugLog("[SneakOnce] Allowed and marked spent. " +
                             "Attacker=" + attacker.CharacterName + " Target=" + (__instance.Target?.CharacterName ?? "null"));
#endif
            }
            catch (Exception ex)
            {
                Log.Error("[SneakOnce] OnTrigger Postfix error", ex);
            }
        }
    }
}
