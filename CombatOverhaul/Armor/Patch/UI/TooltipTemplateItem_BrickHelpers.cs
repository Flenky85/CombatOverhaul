using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Owlcat.Runtime.UI.Tooltips;
using System;
using System.Collections.Generic;

namespace CombatOverhaul.Armor.Patch.UI
{
    internal static class TooltipTemplateItem_BrickHelpers
    {
        public static TooltipBrickIconValueStat Stat(string name, string value) =>
            new TooltipBrickIconValueStat(name, value, null, TooltipIconValueStatType.Normal, null);

        public static TooltipBrickSeparator SepSmall() =>
            new TooltipBrickSeparator(TooltipBrickElementType.Small);

        private static readonly Dictionary<string, string> _glossaryCache = new Dictionary<string, string>();

        /// <summary>
        /// Inserta el bloque en insertIdx si >=0; si no, lo añade al final.
        /// Después, colapsa separadores duplicados.
        /// </summary>
        public static void InsertBlockOrAppend(List<ITooltipBrick> bricks, int insertIdx, List<ITooltipBrick> block)
        {
            if (insertIdx >= 0 && insertIdx <= bricks.Count)
                bricks.InsertRange(insertIdx, block);
            else
                bricks.AddRange(block);

            CoalesceSeparators(bricks);
        }

        /// <summary>
        /// Elimina separadores consecutivos (Small/Normal, etc.).
        /// </summary>
        public static void CoalesceSeparators(List<ITooltipBrick> bricks)
        {
            if (bricks == null || bricks.Count < 2) return;

            for (int i = 1; i < bricks.Count;)
            {
                if (bricks[i] is TooltipBrickSeparator && bricks[i - 1] is TooltipBrickSeparator)
                {
                    bricks.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
        public static string Glossary(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            if (_glossaryCache.TryGetValue(key, out var v)) return v;
            v = UIUtility.GetGlossaryEntryName(key) ?? string.Empty;
            _glossaryCache[key] = v;
            return v;
        }

        /// <summary>
        /// Quita un IconValueStat cuyo nombre coincida con el Glossary(key) y
        /// opcionalmente borra los textos/separador inmediatamente posteriores.
        /// Devuelve true si eliminó algo.
        /// </summary>
        public static bool RemoveIconStatWithTrailingByGlossaryKey(
            List<ITooltipBrick> bricks,
            string glossaryKey,
            bool removeFollowingText = true,
            bool removeFollowingSeparator = true)
        {
            if (bricks == null || bricks.Count == 0) return false;

            var expected = Glossary(glossaryKey);
            if (string.IsNullOrEmpty(expected)) return false;

            for (int i = 0; i < bricks.Count; i++)
            {
                if (bricks[i] is TooltipBrickIconValueStat ivs &&
                    string.Equals(ivs.m_Name ?? string.Empty, expected, StringComparison.Ordinal))
                {
                    bricks.RemoveAt(i);

                    if (removeFollowingText)
                        while (i < bricks.Count && bricks[i] is TooltipBrickText)
                            bricks.RemoveAt(i);

                    if (removeFollowingSeparator && i < bricks.Count && bricks[i] is TooltipBrickSeparator)
                        bricks.RemoveAt(i);

                    CoalesceSeparators(bricks);
                    return true;
                }
            }
            return false;
        }
    }
}
