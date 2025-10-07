using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;

namespace CombatOverhaul.Patches.Features.Commons
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class WeaponFinesse
    {
        private static bool _done;
        private const string WeaponFinesseGuid = "90e54424d682d104ab36436bd527af09";

        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(WeaponFinesseGuid);
            if (feat == null) return;

            var comps = new List<BlueprintComponent>(feat.ComponentsArray);

            // 1) Quitar reemplazos de stat
            for (int i = comps.Count - 1; i >= 0; i--)
                if (comps[i] is AttackStatReplacement) comps.RemoveAt(i);

            // 2) +1 al ataque con armas Finesse (solo melee) — AÑADE name para evitar problemas de serialización
            var atk = new WeaponParametersAttackBonus
            {
                name = "$WeaponParametersAttackBonus$CO_WeaponFinesse", // <— IMPORTANTE
                OnlyFinessable = true,
                CanBeUsedWithFightersFinesse = false,
                Ranged = false,
                OnlyTwoHanded = false,
                UseContextIstead = false,
                AttackBonus = 1,
                Descriptor = ModifierDescriptor.Feat,
                ScaleByBasicAttackBonus = false,
                OnlyForFullAttack = false,
                Multiplier = 1
            };

            comps.Add(atk);
            feat.ComponentsArray = comps.ToArray();

            // 3) Descripción — REUTILIZA LAS KEYS EXISTENTES (no crees keys nuevas)
            //    Si no hay pack aún, no hagas nada (evita NRE).
            var pack = LocalizationManager.CurrentPack;
            if (pack != null)
            {
                var enText =
                    "With a <b><color=#703565><link=\"Encyclopedia:Light_Weapon\">light weapon</link></color></b>, " +
                    "elven curve blade, estoc, or rapier made for a creature of your <b><color=#703565><link=\"Encyclopedia:Size\">size</link></color></b> category, " +
                    "you gain a <b>+1</b> bonus on melee <b><color=#703565><link=\"Encyclopedia:Attack\">attack rolls</link></color></b> made with that weapon.";

                // Usa la key original si existe
                var descKey = feat.m_Description?.m_Key;
                if (!string.IsNullOrEmpty(descKey))
                    pack.PutString(descKey, enText);

                var shortKey = feat.m_DescriptionShort?.m_Key;
                if (!string.IsNullOrEmpty(shortKey))
                    pack.PutString(shortKey, "+1 to melee attack rolls with finesse weapons.");
            }
        }
    }
}
