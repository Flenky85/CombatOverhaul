using HarmonyLib;
using Kingmaker.Items; // ItemEntityArmor
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.MVVM._VM.Tooltip.Utils; // UIUtility.GetGlossaryEntryName
using Kingmaker.UI.Tooltip;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kingmaker.Blueprints.Items.Armors;

namespace CombatOverhaul.UI
{
    [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.GetBody))]
    static class Patch_AddArmorDRBrick
    {
        // Reflection a campos privados de los bricks
        private static readonly FieldInfo NameField =
          AccessTools.Field(typeof(TooltipBrickIconValueStat), "m_Name");
        private static readonly FieldInfo TipField =
          AccessTools.Field(typeof(TooltipBrickIconValueStat), "m_Tooltip");

        // Nombres posibles del campo "clave" dentro de TooltipTemplateGlossary según versión
        private static readonly string[] GlossaryKeyCandidateFields = new[]
        {
      "m_GlossaryKey", "m_Key", "m_Entry", "m_GlossaryEntry", "m_Name"
    };

        static void Postfix(TooltipTemplateItem __instance, TooltipTemplateType type, ref IEnumerable<ITooltipBrick> __result)
        {
            if (__result == null) return;

            var armor = __instance.m_Item as ItemEntityArmor;
            if (armor == null) return;

            int dr = ComputeArmorDR(armor); // tu cálculo real aquí
            if (dr <= 0) return;

            var myBrick = new TooltipBrickIconValueStat(
              name: "Damage Reduction",
              value: $"{dr}%",
              icon: null,
              type: TooltipIconValueStatType.Normal,
              tooltip: null);

            var bricks = __result.ToList();

            // 1) Posición objetivo: ANTES de ArmorCheckPenalty
            int insertIdx = FindBrickIndexByGlossaryKey(bricks, "ArmorCheckPenalty");

            // 2) Fallback por label localizado, por si la clave no es accesible en tu build
            if (insertIdx < 0)
            {
                string acpLabel = UIUtility.GetGlossaryEntryName(TooltipElement.ArmorCheckPenalty.ToString());
                insertIdx = bricks.FindIndex(b =>
                {
                    var s = b as TooltipBrickIconValueStat;
                    if (s == null || NameField == null) return false;
                    var name = NameField.GetValue(s) as string ?? "";
                    return name == acpLabel || name.Contains("Penalizador") || name.Contains("Armor Check");
                });
            }

            if (insertIdx >= 0) bricks.Insert(insertIdx, myBrick);
            else bricks.Add(myBrick); // si no lo encuentra, al final

            __result = bricks;
        }

        private static int FindBrickIndexByGlossaryKey(List<ITooltipBrick> bricks, string key)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                var s = bricks[i] as TooltipBrickIconValueStat;
                if (s == null || TipField == null) continue;

                var tip = TipField.GetValue(s) as TooltipBaseTemplate;
                if (tip == null) continue;

                var t = tip.GetType();
                foreach (var fname in GlossaryKeyCandidateFields)
                {
                    var f = AccessTools.Field(t, fname);
                    if (f == null || f.FieldType != typeof(string)) continue;
                    var k = f.GetValue(tip) as string;
                    if (string.Equals(k, key, StringComparison.Ordinal)) return i;
                }
            }
            return -1;
        }

        private static int ComputeArmorDR(ItemEntityArmor armor)
        {
            var bpArmor = armor?.Blueprint as BlueprintItemArmor;     // blueprint real de la armadura
            int baseReal = bpArmor?.Type?.ArmorBonus ?? 0;            // CA base de la armadura
            return Math.Max(0, baseReal * 5);                         // DR = 5 × base
        }
    }
}
