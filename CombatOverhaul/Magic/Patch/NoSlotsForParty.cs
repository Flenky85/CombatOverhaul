using HarmonyLib;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace CombatOverhaul.Magic.Patch
{
    /// <summary>
    /// Evita que los miembros de la party (y pets) consuman slots al lanzar hechizos,
    /// tanto dentro como fuera de combate. No se aplica a summons ni a aliados externos.
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
            // Solo alteramos cuando realmente se intentaría gastar
            if (!doSpend) return true;

            // Caster desde el AbilityData (cuando doSpend == true debería venir)
            UnitEntityData caster = spell?.Caster?.Unit;
            if (caster == null) return true;

            // Solo hechizos (no SLA/otras abilities)
            if (blueprint == null || !blueprint.IsSpell) return true;

            // Solo party members y pets; fuera aliados y summons
            if (!IsPartyOrPet(caster)) return true;

            // Saltamos el método original -> NO se consumen slots; devolvemos éxito
            __result = true;
            return false;
        }

        private static bool IsPartyOrPet(UnitEntityData unit)
        {
            var player = Game.Instance?.Player;
            if (player == null || unit == null) return false;

            // Aceptar solo miembros de la party y sus pets
            var list = player.PartyAndPets;
            return list != null && list.Contains(unit);
        }
    }
}
