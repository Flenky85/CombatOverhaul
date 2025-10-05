using HarmonyLib;
using Kingmaker.Items;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.MVVM._VM.Tooltip.Utils;
using Kingmaker.UI.Tooltip;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CombatOverhaul.UI
{
    [HarmonyPatch(typeof(TooltipTemplateItem), nameof(TooltipTemplateItem.GetBody))]
    static class Patch_RemoveMaxDex_AndFollowingDetails
    {
        static void Postfix(TooltipTemplateItem __instance, ref IEnumerable<ITooltipBrick> __result)
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
                    toRemove.Add(j);
            }

            if (toRemove.Count > 0)
            {
                var filtered = new List<ITooltipBrick>(bricks.Count - toRemove.Count);
                for (int k = 0; k < bricks.Count; k++)
                    if (!toRemove.Contains(k)) filtered.Add(bricks[k]);
                __result = filtered;
            }
        }

        // --- Detectores sin reflexión ---

        private static bool IsMaxDexIcon(ITooltipBrick brick)
        {
            // Nos centramos en IconValueStat y comparamos por el nombre “bonito” del glosario
            if (brick is TooltipBrickIconValueStat ivs)
            {
                var name = ivs.m_Name ?? string.Empty;

                // Nombre de glosario localizado de MaxDexterity
                // Esto evita depender de campos internos del template
                var expected = UIUtility.GetGlossaryEntryName(TooltipElement.MaxDexterity.ToString());
                if (string.Equals(name, expected, StringComparison.Ordinal))
                    return true;

                // Fallbacks: por si el glosario no coincide en algunos idiomas/mods
                if (name.IndexOf("Max Dexterity", StringComparison.OrdinalIgnoreCase) >= 0) return true;
                if (name.IndexOf("Destreza máxima", StringComparison.OrdinalIgnoreCase) >= 0) return true;
                if (name.IndexOf("Encyclopedia:MaxDexterity", StringComparison.OrdinalIgnoreCase) >= 0) return true;

                // Si algún día necesitas usar el TooltipBaseTemplate:
                // var tip = ivs.m_Tooltip; // acceso directo (publicized)
                // Evitamos inspeccionar campos internos del template para no reintroducir reflexión.
            }

            return false;
        }

        private static bool IsSeparator(ITooltipBrick brick) => brick is TooltipBrickSeparator;

        private static bool IsTextBrick(ITooltipBrick brick) => brick is TooltipBrickText;
    }
}
