using CombatOverhaul.Guids;
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
        
        static void Postfix()
        {
            if (_done) return; _done = true;

            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.WeaponFinesse);
            if (feat == null) return;

            var comps = new List<BlueprintComponent>(feat.ComponentsArray);

            for (int i = comps.Count - 1; i >= 0; i--)
                if (comps[i] is AttackStatReplacement) comps.RemoveAt(i);

            var atk = new WeaponParametersAttackBonus
            {
                name = "$WeaponParametersAttackBonus$CO_WeaponFinesse", 
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

            var pack = LocalizationManager.CurrentPack;
            if (pack != null)
            {
                var enText =
                    "With a <b><color=#703565><link=\"Encyclopedia:Light_Weapon\">light weapon</link></color></b>, " +
                    "elven curve blade, estoc, or rapier made for a creature of your <b><color=#703565><link=\"Encyclopedia:Size\">size</link></color></b> category, " +
                    "you gain a <b>+1</b> bonus on melee <b><color=#703565><link=\"Encyclopedia:Attack\">attack rolls</link></color></b> made with that weapon.";

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
