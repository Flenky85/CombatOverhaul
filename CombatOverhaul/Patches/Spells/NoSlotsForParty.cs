using HarmonyLib;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace CombatOverhaul.Patches.Spells
{
    /// <summary>
    /// Evita que los miembros de la party (y pets) consuman slots al lanzar hechizos,
    /// solo durante combate. No modifica blueprints.
    /// Se engancha en Spellbook.SpendInternal(...) que es donde Owlcat descuenta slots.
    /// </summary>
    [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.SpendInternal))]
    internal static class NoSlotsForParty
    {
        // Firma en tu build:
        // bool SpendInternal(BlueprintAbility blueprint, AbilityData spell, bool doSpend, bool excludeSpecial=false)
        static bool Prefix(
            BlueprintAbility blueprint,
            AbilityData spell,
            bool doSpend,
            ref bool __result)
        {
            // Solo alteramos en el momento de gastar
            if (!doSpend) return true;

            // Necesitamos el caster desde 'spell' (cuando doSpend == true Owlcat lo exige) :contentReference[oaicite:1]{index=1}
            UnitEntityData caster = spell?.Caster?.Unit;
            if (caster == null) return true;

            // Solo party/pets y en combate
            if (!caster.IsPlayerFaction || !caster.IsInCombat || Game.Instance?.Player?.IsInCombat != true)
                return true;

            // Solo hechizos (no SLA/otras abilities)
            if (blueprint == null || !blueprint.IsSpell) return true;

            // Saltamos el método original -> NO se consumen slots; devolvemos éxito
            __result = true;
            return false;
        }
    }
}
