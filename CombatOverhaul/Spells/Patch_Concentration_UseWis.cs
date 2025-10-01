/*using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules.Abilities;

namespace CombatOverhaul.Spells
{
    /// La Concentration usa SIEMPRE el bonus de WIS (post-ajuste del valor usado en la tirada).
    [HarmonyPatch(typeof(RuleCheckConcentration), nameof(RuleCheckConcentration.OnTrigger))]
    internal static class Patch_CheckConcentration_ForceWis
    {
        static void Postfix(RuleCheckConcentration __instance)
        {
            var caster = __instance?.Initiator;
            if (caster == null) return;

            // Asumimos que el cálculo base ha usado CHA si llevas el parche de DC=CHA.
            // Cambiamos la parte del atributo: +WIS - CHA. Se preservan CL y otros bonos ya sumados.
            int cha = caster.Stats.Charisma?.Bonus ?? 0;
            int wis = caster.Stats.Wisdom?.Bonus ?? 0;

            int delta = wis - cha;
            if (delta != 0)
                __instance.Concentration += delta;
        }
    }
}
*/