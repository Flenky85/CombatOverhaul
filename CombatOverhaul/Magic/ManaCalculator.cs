using System;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.EntitySystem.Stats;

namespace CombatOverhaul.Magic
{
    internal struct CasterBuckets
    {
        public int CL10;   
        public int CL6;    
        public int CL4;    
        public int Other;  
        public int BestCastingMod; 
    }

    internal static class ManaCalc
    {
        private const int WEIGHT_L10 = 5;
        private const int WEIGHT_L6 = 3;
        private const int WEIGHT_L4 = 2;

        private const float BONUS_PCT_PER_MOD = 0.00f; 

        private const float REGEN_PCT_OF_MAX = 0.20f;  
        private const int REGEN_MIN_FLAT = 1;

        public static CasterBuckets GetBuckets(UnitEntityData unit)
        {
            var r = new CasterBuckets();
            if (unit == null || unit.Descriptor == null) return r;

            var books = unit.Descriptor.Spellbooks;
            if (books == null) return r;

            int bestMod = int.MinValue;

            foreach (var sb in books)
            {
                if (sb == null || sb.Blueprint == null) continue;
                if (sb.IsMythic) continue; 

                int cl = sb.CasterLevel;
                if (cl < 0) cl = 0;

                int cap = GetBucketCapFromBlueprint(sb);
                if (cap >= 9) r.CL10 += cl;   
                else if (cap == 6) r.CL6 += cl;
                else if (cap == 4) r.CL4 += cl;
                else r.Other += cl;

                var stat = GetCastingStatSafe(sb);
                int mod = GetStatMod(unit, stat);
                if (mod > bestMod) bestMod = mod;
            }

            r.BestCastingMod = bestMod == int.MinValue ? 0 : bestMod;
            return r;
        }

        public static int CalcMaxMana(UnitEntityData unit)
        {
            var b = GetBuckets(unit);

            int baseSum = b.CL10 * WEIGHT_L10 + b.CL6 * WEIGHT_L6 + b.CL4 * WEIGHT_L4;
            if (baseSum <= 0) return 0;

            int mod = b.BestCastingMod;
            if (mod < 0) mod = 0; 

            float pct = 1f + mod * BONUS_PCT_PER_MOD;
            int result = (int)Math.Round(baseSum * pct, MidpointRounding.AwayFromZero);
            if (result < 0) result = 0;
            return (int)((result + 10) * 1.5f);
        }

        public static int CalcManaPerTurn(UnitEntityData unit, int? maxManaOpt = null)
        {
            int max = maxManaOpt ?? CalcMaxMana(unit);
            if (max <= 0) return 0;

            int regen = (int)Math.Round(max * REGEN_PCT_OF_MAX, MidpointRounding.AwayFromZero);
            if (regen < REGEN_MIN_FLAT) regen = REGEN_MIN_FLAT;
            return regen;
        }

        private static void BucketByMaxLevel(ref CasterBuckets b, int maxLvl, int cl)
        {
            if (maxLvl >= 9) b.CL10 += cl; 
            else if (maxLvl == 6) b.CL6 += cl;
            else if (maxLvl == 4) b.CL4 += cl;
            else b.Other += cl;
        }

        private static int GetMaxSpellLevelSafe(Spellbook sb)
        {
            try
            {
                int lvl = sb.MaxSpellLevel; 
                if (lvl >= 0) return lvl;
            }
            catch { }

            try
            {
                int lvl = sb.Blueprint != null ? sb.Blueprint.MaxSpellLevel : 0;
                if (lvl > 0) return lvl;
            }
            catch { }

            try
            {
                var list = sb.Blueprint?.SpellList;
                if (list?.SpellsByLevel != null)
                {
                    int max = 0;
                    for (int i = 0; i < list.SpellsByLevel.Length; i++)
                    {
                        var e = list.SpellsByLevel[i];
                        if (e != null && e.Spells != null && e.Spells.Count > 0)
                            if (e.SpellLevel > max) max = e.SpellLevel;
                    }
                    return max;
                }
            }
            catch { }

            return 0;
        }

        private static int GetBucketCapFromBlueprint(Spellbook sb)
        {
            try
            {
                if (sb != null && sb.Blueprint != null)
                {
                    int cap = sb.Blueprint.MaxSpellLevel;
                    if (cap > 0) return cap;
                }
            }
            catch { }

            try
            {
                var bp = sb?.Blueprint;
                var list = bp?.SpellList;
                if (list != null && list.SpellsByLevel != null)
                {
                    int max = 0;
                    for (int i = 0; i < list.SpellsByLevel.Length; i++)
                    {
                        var lvl = list.SpellsByLevel[i];
                        if (lvl != null && lvl.Spells != null && lvl.Spells.Count > 0)
                            if (lvl.SpellLevel > max) max = lvl.SpellLevel;
                    }
                    return max;
                }
            }
            catch { }
            return 0;
        }

        private static StatType GetCastingStatSafe(Spellbook sb)
        {
            try
            {
                if (sb.Blueprint != null)
                {
                    var st = sb.Blueprint.CastingAttribute;
                    if (st != StatType.Unknown) return st;
                }
            }
            catch { }
            return StatType.Wisdom;
        }

        private static int GetStatMod(UnitEntityData unit, StatType stat)
        {
            if (unit == null || unit.Descriptor == null) return 0;
            if (stat == StatType.Unknown) return 0;

            try
            {
                var s = unit.Descriptor.Stats.GetStat(stat);
                if (s == null) return 0;

                int score = s.ModifiedValue; 
                int mod = (int)Math.Floor((score - 10) / 2.0);
                return mod;
            }
            catch
            {
                return 0;
            }
        }
    }
}
