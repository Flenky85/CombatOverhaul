using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules.Abilities;

namespace CombatOverhaul.Patches.Magic
{
    [HarmonyPatch(typeof(RuleCalculateAbilityParams), nameof(RuleCalculateAbilityParams.OnTrigger))]
    internal static class DC_ForceCHA_Conc_ForceWis
    {
        private static void Prefix(RuleCalculateAbilityParams __instance)
        {
            var caster = __instance?.Initiator;
            if (caster == null) return;

            __instance.ReplaceStat = StatType.Charisma;
            __instance.ReplaceStatBonusModifier = caster.Stats.Charisma?.Bonus ?? 0;
        }

        private static void Postfix(RuleCalculateAbilityParams __instance)
        {
            var caster = __instance?.Initiator;
            var res = __instance?.Result;
            if (caster == null || res == null) return;

            int cl = res.CasterLevel;
            int cha = caster.Stats.Charisma?.Bonus ?? 0;
            int wis = caster.Stats.Wisdom?.Bonus ?? 0;

            int extras = res.Concentration - (cl + cha);

            res.Concentration = cl + wis + extras;
        }
    }
}
