using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;                 // BlueprintFeature
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.Localization;                       // LocalizationManager, LocalizedString
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;

namespace CombatOverhaul.Patches.Features
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

            // 1) Quitar los reemplazos de stat (ya no nos hacen falta en tu sistema)
            for (int i = comps.Count - 1; i >= 0; i--)
                if (comps[i] is AttackStatReplacement) comps.RemoveAt(i);

            // 2) +1 al ataque con armas Finesse (solo melee)
            var atk = new WeaponParametersAttackBonus
            {
                OnlyFinessable = true,                 // requiere arma Finessable
                CanBeUsedWithFightersFinesse = false,  // ponlo a true y asigna m_FightersFinesse si quieres incluir Fighter's Finesse
                Ranged = false,                        // solo melee
                OnlyTwoHanded = false,
                UseContextIstead = false,
                AttackBonus = 1,
                Descriptor = ModifierDescriptor.Feat,  // evita apilados raros
                ScaleByBasicAttackBonus = false,
                OnlyForFullAttack = false,
                Multiplier = 1
            };

            comps.Add(atk);
            feat.ComponentsArray = comps.ToArray();

            // 3) Actualizar descripción
            const string KEY = "CO_WeaponFinesse_Desc";
            const string KEY_SHORT = "CO_WeaponFinesse_Desc_Short";

            var enText =
                "With a <b><color=#703565><link=\"Encyclopedia:Light_Weapon\">light weapon</link></color></b>, " +
                "elven curve blade, estoc, or rapier made for a creature of your <b><color=#703565><link=\"Encyclopedia:Size\">size</link></color></b> category, " +
                "you gain a <b>+1</b> bonus on melee <b><color=#703565><link=\"Encyclopedia:Attack\">attack rolls</link></color></b> made with that weapon.";

            LocalizationManager.CurrentPack.PutString(KEY, enText);
            LocalizationManager.CurrentPack.PutString(KEY_SHORT, "+1 to melee attack rolls with finesse weapons.");

            feat.m_Description = new LocalizedString { m_Key = KEY };
            feat.m_DescriptionShort = new LocalizedString { m_Key = KEY_SHORT };
        }
    }
}
