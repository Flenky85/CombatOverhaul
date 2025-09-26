/*using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using System.Linq;
using System.Reflection;

namespace CombatOverhaul.Patches
{
    [HarmonyPatch(typeof(RuleCalculateWeaponStats), nameof(RuleCalculateWeaponStats.OnTrigger))]
    internal static class WeaponStats_StrAsPercent_UIFriendly
    {
        // ---- TU TABLA POR CADA +1 de STR ----
        private const float PCT_PER_POINT_2H = 0.30f; // 30% por +1 STR
        private const float PCT_PER_POINT_1H = 0.20f; // 20% por +1 STR (mano principal)
        private const float PCT_PER_POINT_OFF = 0.15f; // 15% por +1 STR (mano secundaria)

        // Naturales: porcentaje POR CADA +1 de STR según nº total de ataques naturales
        private static float GetPctPerPointNatural(int naturalsCount, bool withManufactured)
        {
            if (withManufactured)
            {
                // “Empieza en 15%”: solo cambia el caso de 1 natural
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
                default:
                    return 0.03f;
            }
        }

        static void Prefix(RuleCalculateWeaponStats __instance)
        {
            var actor = __instance?.Initiator;
            var wep = __instance?.Weapon;
            if (actor == null || wep == null) return;

            // 1) STR del actor
            int strMod = actor.Stats?.Strength?.Bonus ?? 0;
            if (strMod <= 0) return; // si no hay bono positivo, no cambiamos nada

            // 2) Categoría: 2H / Offhand / Natural / 1H
            bool isTwoHand = wep.HoldInTwoHands || (wep.Blueprint?.IsTwoHanded ?? false);
            bool isOffhand = IsOffhand(actor, wep);
            bool isNatural = wep.Blueprint?.IsNatural ?? false;

            // 3) Porcentaje por punto de STR según tabla
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

            // 4) Media del daño base del arma (N * (1+M)/2)
            float avgBase = GetWeaponBaseAverage(wep);
            if (avgBase <= 0f) return;

            // 5) Queremos un bono plano final = floor( avgBase * perPoint * strMod )
            //    El sistema calcula: num2 = STR_mod * DamageBonusStatMultiplier (para positivos)
            //    => ponemos OverrideDamageBonus = (avgBase * perPoint)  (esto * STR_mod = extra deseado)
            float desiredPerPointFlat = avgBase * perPoint;

            // Evita interferencias de la lógica vanilla (offhand/2H/natural) porque Override manda
            __instance.OverrideDamageBonus = desiredPerPointFlat;
        }

        // ----------------- Helpers robustos -----------------

        private static bool IsOffhand(UnitEntityData actor, ItemEntityWeapon wep)
            => actor?.Body?.SecondaryHand?.MaybeWeapon == wep;

        private static int CountNaturalAttacks(UnitEntityData actor)
        {
            if (actor?.Body == null) return 0;
            int count = 0;

            var main = actor.Body.PrimaryHand?.MaybeWeapon;
            if (main != null && main.Blueprint?.IsNatural == true && !main.Blueprint.IsUnarmed) count++;

            var off = actor.Body.SecondaryHand?.MaybeWeapon;
            if (off != null && off.Blueprint?.IsNatural == true && !off.Blueprint.IsUnarmed) count++;

            var limbs = actor.Body.AdditionalLimbs;
            if (limbs != null)
                foreach (var slot in limbs)
                {
                    var w = slot?.MaybeWeapon;
                    if (w != null && w.Blueprint?.IsNatural == true && !w.Blueprint.IsUnarmed) count++;
                }

            return count;
        }

        private static bool HasAnyManufactured(UnitEntityData actor)
        {
            if (actor?.Body == null) return false;

            bool IsManufactured(ItemEntityWeapon w)
                => w != null && (w.Blueprint?.IsNatural != true);

            var main = actor.Body.PrimaryHand?.MaybeWeapon;
            var off = actor.Body.SecondaryHand?.MaybeWeapon;

            if (IsManufactured(main)) return true;
            if (IsManufactured(off)) return true;

            return false;
        }


        /// <summary>
        /// Media del daño base del arma (solo el dado base del arma).
        /// Intenta leer de Weapon.Blueprint.BaseDamage (DiceFormula) por reflexión.
        /// </summary>
        private static float GetWeaponBaseAverage(ItemEntityWeapon wep)
        {
            var bp = wep?.Blueprint;
            if (bp == null) return 0f;

            // En tu build, el ctor hace: new ModifiableDiceFormula(Weapon.Blueprint.BaseDamage)
            // Así que BaseDamage es un DiceFormula con N y Tipo de Dado.
            object baseDamage = bp.GetType().GetProperty("BaseDamage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(bp);
            if (baseDamage == null) return 0f;

            // Campos habituales en DiceFormula: m_Rolls (int), m_Dice (DiceType)
            int rolls = ReadIntField(baseDamage, "m_Rolls");
            int sides = GetDiceSides(ReadEnumField<DiceType>(baseDamage, "m_Dice"));

            if (rolls <= 0 || sides <= 0) return 0f;

            // Media NdM = N * (1+M) / 2
            return rolls * (1 + sides) * 0.5f;
        }

        private static int ReadIntField(object obj, string name)
        {
            var f = obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f == null) return 0;
            var v = f.GetValue(obj);
            return v is int i ? i : 0;
        }

        private static TEnum ReadEnumField<TEnum>(object obj, string name) where TEnum : struct
        {
            var f = obj.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f == null) return default;
            var v = f.GetValue(obj);
            return v is TEnum e ? e : default;
        }

        private static int GetDiceSides(DiceType dice)
        {
            // En PF/WotR, el valor numérico del enum suele ser los lados (D3=3, D4=4, D6=6, etc.)
            int sides = (int)(object)dice;
            return sides > 0 ? sides : 0;
        }
    }
}*/
