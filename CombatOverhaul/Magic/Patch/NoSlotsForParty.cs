using CombatOverhaul.Utils;            
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace CombatOverhaul.Magic.Patch
{
    [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.SpendInternal))]
    internal static class NoSlotsForParty
    {
        static bool Prefix(
            BlueprintAbility blueprint,
            AbilityData spell,
            bool doSpend,
            ref bool __result)
        {
            if (!doSpend) return true;

            UnitEntityData caster = spell?.Caster?.Unit;
            if (caster == null) return true;

            if (blueprint == null || !blueprint.IsSpell) return true;

            if (!PartyUtils.IsPartyOrPet(caster)) return true;

            __result = true;
            return false;
        }
    }
}
