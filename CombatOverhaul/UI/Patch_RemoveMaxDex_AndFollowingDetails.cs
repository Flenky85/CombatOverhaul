using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.Tooltip;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CombatOverhaul.UI
{
    [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.GetBody))]
    static class Patch_RemoveMaxDex_AndFollowingDetails
    {
        private static readonly string[] StringFieldCandidates = { "m_Name", "m_Text", "m_Value" };

        static void Postfix(TooltipTemplateItem __instance, TooltipTemplateType type, ref IEnumerable<ITooltipBrick> __result)
        {
            if (__result == null) return;
            if (!(__instance.m_Item is ItemEntityArmor)) return;

            var bricks = __result.ToList();
            var toRemove = new HashSet<int>();

            for (int i = 0; i < bricks.Count; i++)
            {
                if (!IsMaxDexIcon(bricks[i])) continue;

                // 1) borra el propio MaxDex icon brick
                toRemove.Add(i);

                // 2) borra todos los Texts siguientes (MaxDexterityDetails)
                int j = i + 1;
                while (j < bricks.Count && IsTextBrick(bricks[j]))
                {
                    toRemove.Add(j);
                    j++;
                }

                // 3) borra el primer Separator inmediatamente después de los Details
                if (j < bricks.Count && IsSeparator(bricks[j]))
                {
                    toRemove.Add(j);
                }
            }

            if (toRemove.Count > 0)
            {
                var filtered = new List<ITooltipBrick>(bricks.Count - toRemove.Count);
                for (int k = 0; k < bricks.Count; k++)
                    if (!toRemove.Contains(k)) filtered.Add(bricks[k]);
                __result = filtered;
            }
        }

        // --- Detectores ---

        private static bool IsMaxDexIcon(ITooltipBrick brick)
        {
            if (brick == null) return false;

            var tip = GetAnyTooltipTemplate(brick);
            if (tip != null)
            {
                var elem = GetAnyTooltipElement(tip);
                if (elem.HasValue && elem.Value == TooltipElement.MaxDexterity) return true;

                var key = GetAnyStringField(tip, "m_GlossaryKey", "m_Key", "m_Entry", "m_GlossaryEntry", "m_Name");
                if (string.Equals(key, "MaxDexterity", StringComparison.Ordinal)) return true;
            }

            var text = GetAnyStringField(brick, StringFieldCandidates);
            if (!string.IsNullOrEmpty(text))
            {
                if (text.IndexOf("Encyclopedia:MaxDexterity", StringComparison.OrdinalIgnoreCase) >= 0) return true;
                if (text.IndexOf("Destreza máxima", StringComparison.OrdinalIgnoreCase) >= 0) return true;
                if (text.IndexOf("Max Dexterity", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            }

            return false;
        }

        private static bool IsSeparator(ITooltipBrick brick)
            => brick != null && brick.GetType().Name.EndsWith("TooltipBrickSeparator", StringComparison.Ordinal);

        private static bool IsTextBrick(ITooltipBrick brick)
            => brick != null && brick.GetType().Name.EndsWith("TooltipBrickText", StringComparison.Ordinal);

        // --- Reflexión utilitaria ---

        private static TooltipBaseTemplate GetAnyTooltipTemplate(ITooltipBrick brick)
        {
            if (brick == null) return null;
            var fields = brick.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var f in fields)
                if (typeof(TooltipBaseTemplate).IsAssignableFrom(f.FieldType))
                    return f.GetValue(brick) as TooltipBaseTemplate;
            return null;
        }

        private static TooltipElement? GetAnyTooltipElement(TooltipBaseTemplate tip)
        {
            if (tip == null) return null;
            var fields = tip.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var f in fields)
                if (f.FieldType == typeof(TooltipElement))
                    return (TooltipElement)f.GetValue(tip);
            return null;
        }

        private static string GetAnyStringField(object obj, params string[] names)
        {
            if (obj == null) return null;
            var t = obj.GetType();
            foreach (var n in names)
            {
                var f = t.GetField(n, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (f != null && f.FieldType == typeof(string))
                {
                    var s = f.GetValue(obj) as string;
                    if (!string.IsNullOrEmpty(s)) return s;
                }
            }
            return null;
        }
    }
}
