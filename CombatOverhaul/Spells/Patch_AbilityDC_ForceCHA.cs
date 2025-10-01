using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums; // StatType
using Kingmaker.RuleSystem.Rules.Abilities;

namespace CombatOverhaul.Spells
{
    // Mantén tu Prefix tal cual (CHA para CD y stat “oficial”)
    [HarmonyPatch(typeof(RuleCalculateAbilityParams), nameof(RuleCalculateAbilityParams.OnTrigger))]
    internal static class Patch_AbilityDC_ForceCHA
    {
        static void Prefix(RuleCalculateAbilityParams __instance)
        {
            var caster = __instance?.Initiator;
            if (caster == null) return;

            __instance.ReplaceStatBonusModifier = caster.Stats.Charisma.Bonus;
            __instance.ReplaceStat = StatType.Charisma;
        }

        // Y añade este Postfix para forzar WIS SOLO en Concentration
        static void Postfix(RuleCalculateAbilityParams __instance)
        {
            var caster = __instance?.Initiator;
            var res = __instance?.Result;
            if (caster == null || res == null) return;

            // En Prefix hemos fijado que el stat usado fue CHA.
            int cha = caster.Stats.Charisma?.Bonus ?? 0;
            int wis = caster.Stats.Wisdom?.Bonus ?? 0;

            // Extras ya aplicados por el juego (bonos de concentración que no son el stat):
            // res.Concentration = res.CasterLevel + CHA + extras  →  extras = actual - (CL + CHA)
            int extras = res.Concentration - (res.CasterLevel + cha);

            // Sustituimos CHA por WIS manteniendo CL y extras
            res.Concentration = res.CasterLevel + wis + extras;
        }
    }

}
