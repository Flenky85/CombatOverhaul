using System;
using System.Text;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints.Classes.Spells; // <-- BlueprintSpellbook
using UnityEngine;

namespace CombatOverhaul.Testing
{
    // Uso: CombatOverhaul.Testing.SpellbookDump.DumpPartySpellbooks();
    internal static class SpellbookDump
    {
        public static void DumpPartySpellbooks()
        {
            try
            {
                var game = Game.Instance;
                if (game == null || game.Player == null)
                {
                    Debug.Log("[CO][Dump] Game/Player null.");
                    return;
                }

                var party = game.Player.PartyAndPets;
                if (party == null)
                {
                    Debug.Log("[CO][Dump] PartyAndPets null.");
                    return;
                }

                Debug.Log($"[CO][Dump] === PARTY SPELLBOOK DUMP (count={party.Count}) ===");

                for (int i = 0; i < party.Count; i++)
                {
                    var unit = party[i];
                    if (unit == null || unit.Descriptor == null)
                    {
                        Debug.Log("[CO][Dump] Unit null/descriptor null, skip.");
                        continue;
                    }

                    DumpUnitSpellbooks(unit);
                }

                Debug.Log("[CO][Dump] === END PARTY SPELLBOOK DUMP ===");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Dump] EX: {ex}");
            }
        }

        private static void DumpUnitSpellbooks(UnitEntityData unit)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"[CO][Dump] --- Unit: {SafeName(unit)} ---");

                // Mods de atributos típicos
                int intScore = GetScore(unit, StatType.Intelligence);
                int wisScore = GetScore(unit, StatType.Wisdom);
                int chaScore = GetScore(unit, StatType.Charisma);
                int intMod = ScoreToMod(intScore);
                int wisMod = ScoreToMod(wisScore);
                int chaMod = ScoreToMod(chaScore);

                sb.AppendLine($"[CO][Dump] Stats  INT={intScore}({intMod:+#;-#;0})  WIS={wisScore}({wisMod:+#;-#;0})  CHA={chaScore}({chaMod:+#;-#;0})");

                var books = unit.Descriptor.Spellbooks; // IEnumerable<Spellbook>
                if (books == null)
                {
                    sb.AppendLine("[CO][Dump] No spellbooks.");
                    Debug.Log(sb.ToString());
                    return;
                }

                int bucket10 = 0, bucket6 = 0, bucket4 = 0, bucketOther = 0;

                foreach (var book in books)
                {
                    if (book == null)
                    {
                        sb.AppendLine("[CO][Dump]  * spellbook=null");
                        continue;
                    }

                    var bp = book.Blueprint; // BlueprintSpellbook
                    string sbName = bp != null ? bp.name : "NULL_BP";
                    int cl = Safe(() => book.CasterLevel, 0);
                    bool isMythic = Safe(() => book.IsMythic, false);

                    // MaxSpellLevel (runtime invoca GetMaxSpellLevel), blueprint y deducido
                    int maxLvlRT = Safe(() => book.MaxSpellLevel, 0);
                    int maxLvlBP = Safe(() => bp != null ? bp.MaxSpellLevel : 0, 0);
                    int maxLvlList = DeduceMaxFromList(bp);

                    int lastLvl = Safe(() => book.LastSpellbookLevel, -1);
                    var castAttr = Safe(() => bp != null ? bp.CastingAttribute : StatType.Unknown, StatType.Unknown);
                    string knownByLvl = CountKnownSpellsByLevel(book);

                    sb.AppendLine($"[CO][Dump]  * Book='{sbName}'  Mythic={isMythic}  CL={cl}");
                    sb.AppendLine($"[CO][Dump]    MaxSpellLevel: Runtime={maxLvlRT}  BP={maxLvlBP}  DeducedList={maxLvlList}  LastSpellbookLevel={lastLvl}");
                    sb.AppendLine($"[CO][Dump]    CastingAttribute={castAttr}");
                    if (!string.IsNullOrEmpty(knownByLvl))
                        sb.AppendLine($"[CO][Dump]    KnownSpells: {knownByLvl}");

                    int maxLvl = maxLvlRT > 0 ? maxLvlRT : (maxLvlBP > 0 ? maxLvlBP : maxLvlList);

                    // Bucket: ahora "full caster" = >= 9 (incluye 10 si está el flag)
                    if (!isMythic)
                    {
                        if (maxLvl >= 9) bucket10 += Math.Max(0, cl);
                        else if (maxLvl == 6) bucket6 += Math.Max(0, cl);
                        else if (maxLvl == 4) bucket4 += Math.Max(0, cl);
                        else bucketOther += Math.Max(0, cl);
                    }
                }

                sb.AppendLine($"[CO][Dump] Buckets (non-mythic): L10or9={bucket10}  L6={bucket6}  L4={bucket4}  Other={bucketOther}");
                Debug.Log(sb.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CO][Dump] DumpUnitSpellbooks('{SafeName(unit)}') EX: {ex}");
            }
        }

        private static string SafeName(UnitEntityData u)
        {
            try { return u?.CharacterName ?? "NULL"; } catch { return "NULL"; }
        }

        private static int GetScore(UnitEntityData u, StatType stat)
        {
            try
            {
                var s = u?.Descriptor?.Stats?.GetStat(stat);
                return s != null ? s.ModifiedValue : 0;
            }
            catch { return 0; }
        }

        private static int ScoreToMod(int score)
        {
            return (int)Math.Floor((score - 10) / 2.0);
        }

        private static T Safe<T>(Func<T> f, T fallback)
        {
            try { return f(); } catch { return fallback; }
        }

        private static int DeduceMaxFromList(BlueprintSpellbook bp) // <-- tipo correcto
        {
            try
            {
                if (bp == null || bp.SpellList == null || bp.SpellList.SpellsByLevel == null) return 0;
                int max = 0;
                var arr = bp.SpellList.SpellsByLevel;
                for (int i = 0; i < arr.Length; i++)
                {
                    var lvl = arr[i];
                    if (lvl != null && lvl.Spells != null && lvl.Spells.Count > 0)
                    {
                        if (lvl.SpellLevel > max) max = lvl.SpellLevel;
                    }
                }
                return max;
            }
            catch { return 0; }
        }

        private static string CountKnownSpellsByLevel(Spellbook book)
        {
            try
            {
                int levels = 10;
                try
                {
                    int rt = book.MaxSpellLevel;
                    if (rt + 1 > levels) levels = rt + 1;
                }
                catch { }

                var sb = new StringBuilder();
                bool any = false;

                for (int lvl = 0; lvl <= levels; lvl++)
                {
                    int count = 0;
                    try
                    {
                        var known = book.GetKnownSpells(lvl);
                        if (known != null) count = known.Count;
                    }
                    catch { /* ignore */ }

                    if (count > 0)
                    {
                        if (any) sb.Append(", ");
                        sb.AppendFormat("{0}:{1}", lvl, count);
                        any = true;
                    }
                }

                return any ? sb.ToString() : "";
            }
            catch { return ""; }
        }
    }
}
