using System.Reflection;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class StrengthDamageCalculator
    {
        // ---- TABLA POR CADA +1 de STR (idéntica a la original) ----
        private const float PCT_PER_POINT_2H = 0.30f;
        private const float PCT_PER_POINT_1H = 0.20f;
        private const float PCT_PER_POINT_OFF = 0.15f;

        // Naturales: porcentaje POR CADA +1 de STR según nº total de ataques naturales
        private static float GetPctPerPointNatural(int naturalsCount, bool withManufactured)
        {
            if (withManufactured)
            {
                if (naturalsCount <= 1) return 0.15f;
                // Para 2+ seguimos la tabla normal
            }

            switch (naturalsCount)
            {
                case 1: return 0.30f;
                case 2: return 0.15f;
                case 3: return 0.10f;
                case 4: return 0.075f;
                case 5: return 0.06f;
                case 6: return 0.05f;
                case 7: return 0.0429f;
                case 8: return 0.0375f;
                case 9: return 0.0334f;
                case 10: return 0.03f;
                default: return 0.03f;
            }
        }

        /// <summary>
        /// Calcula el valor para asignar a OverrideDamageBonus de forma que
        /// OverrideDamageBonus * STR_mod == bono plano deseado.
        /// Devuelve 0 si no aplica (p.ej. STR_mod <= 0 o daño base inválido).
        /// </summary>
        public static float ComputeOverrideDamagePerPoint(UnitEntityData actor, ItemEntityWeapon weapon)
        {
            if (actor == null || weapon == null) return 0f;

            int strMod = actor.Stats != null && actor.Stats.Strength != null ? actor.Stats.Strength.Bonus : 0;
            if (strMod <= 0) return 0f;

            bool isTwoHand = weapon.HoldInTwoHands || (weapon.Blueprint != null && weapon.Blueprint.IsTwoHanded);
            bool isOffhand = IsOffhand(actor, weapon);
            bool isNatural = weapon.Blueprint != null && weapon.Blueprint.IsNatural;

            float perPoint;
            if (isNatural)
            {
                int naturals = CountNaturalAttacks(actor);
                bool hasManufactured = HasAnyManufactured(actor);
                perPoint = GetPctPerPointNatural(naturals, hasManufactured);
            }
            else if (isTwoHand) perPoint = PCT_PER_POINT_2H;
            else if (isOffhand) perPoint = PCT_PER_POINT_OFF;
            else perPoint = PCT_PER_POINT_1H;

            float avgBase = GetWeaponBaseAverage(weapon);
            if (avgBase <= 0f) return 0f;

            return avgBase * perPoint; // esto multiplicado por STR_mod = bono plano final
        }

        // ----------------- Helpers (idénticos) -----------------

        private static bool IsOffhand(UnitEntityData actor, ItemEntityWeapon wep)
        {
            return actor != null
                && actor.Body != null
                && actor.Body.SecondaryHand != null
                && actor.Body.SecondaryHand.MaybeWeapon == wep;
        }

        private static int CountNaturalAttacks(UnitEntityData actor)
        {
            if (actor == null || actor.Body == null) return 0;
            int count = 0;

            var main = actor.Body.PrimaryHand != null ? actor.Body.PrimaryHand.MaybeWeapon : null;
            if (main != null && main.Blueprint != null && main.Blueprint.IsNatural && !main.Blueprint.IsUnarmed) count++;

            var off = actor.Body.SecondaryHand != null ? actor.Body.SecondaryHand.MaybeWeapon : null;
            if (off != null && off.Blueprint != null && off.Blueprint.IsNatural && !off.Blueprint.IsUnarmed) count++;

            var limbs = actor.Body.AdditionalLimbs;
            if (limbs != null)
            {
                foreach (var slot in limbs)
                {
                    var w = slot != null ? slot.MaybeWeapon : null;
                    if (w != null && w.Blueprint != null && w.Blueprint.IsNatural && !w.Blueprint.IsUnarmed) count++;
                }
            }

            return count;
        }

        private static bool HasAnyManufactured(UnitEntityData actor)
        {
            if (actor == null || actor.Body == null) return false;

            var main = actor.Body.PrimaryHand != null ? actor.Body.PrimaryHand.MaybeWeapon : null;
            var off = actor.Body.SecondaryHand != null ? actor.Body.SecondaryHand.MaybeWeapon : null;

            if (IsManufactured(main)) return true;
            if (IsManufactured(off)) return true;

            return false;
        }

        private static bool IsManufactured(ItemEntityWeapon w)
        {
            return w != null && (w.Blueprint == null || !w.Blueprint.IsNatural);
        }

        /// <summary>
        /// Media del daño base del arma (solo el dado base del arma).
        /// Lee Weapon.Blueprint.BaseDamage (DiceFormula) por reflexión.
        /// </summary>
        private static float GetWeaponBaseAverage(ItemEntityWeapon wep)
        {
            var bp = wep != null ? wep.Blueprint : null;
            if (bp == null) return 0f;

            var pi = bp.GetType().GetProperty("BaseDamage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            object baseDamage = pi != null ? pi.GetValue(bp) : null;
            if (baseDamage == null) return 0f;

            int rolls = ReadIntField(baseDamage, "m_Rolls");
            int sides = GetDiceSides(ReadEnumField<DiceType>(baseDamage, "m_Dice"));

            if (rolls <= 0 || sides <= 0) return 0f;
            return rolls * (1 + sides) * 0.5f; // N * (1+M)/2
        }

        private static int ReadIntField(object obj, string name)
        {
            if (obj == null) return 0;
            var f = obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f == null) return 0;
            var v = f.GetValue(obj);
            return v is int ? (int)v : 0;
        }

        private static TEnum ReadEnumField<TEnum>(object obj, string name) where TEnum : struct
        {
            if (obj == null) return default(TEnum);
            var f = obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f == null) return default(TEnum);
            var v = f.GetValue(obj);
            return v is TEnum ? (TEnum)v : default(TEnum);
        }

        private static int GetDiceSides(DiceType dice)
        {
            int sides = (int)(object)dice; // enum → int
            return sides > 0 ? sides : 0;
        }
    }
}
