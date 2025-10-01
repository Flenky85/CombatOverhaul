using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using System;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class ArmorCalculator
    {
        // ============ ARMOR BASE ============
        // Lee el "armor bonus base" que define el catálogo (Type), con fallback al del item.
        public static int GetArmorBase(ItemEntityArmor armorItem)
        {
            if (armorItem == null) return 0;
            var bp = armorItem.Blueprint as BlueprintItemArmor;
            if (bp == null) return 0;

            // Prioriza el bonus del Type (suele ser el “base real” del modelo de armadura).
            var fromType = bp.Type != null ? bp.Type.ArmorBonus : 0;
            var fromItem = bp.ArmorBonus;
            return fromType > 0 ? fromType : fromItem;
        }

        // RD base: 5% por punto de armor base.
        public static float GetBaseRdPercentFromArmorBase(int armorBase)
        {
            return armorBase > 0 ? armorBase * 0.05f : 0f;
        }

        // Escalado RD por tipo (100% físico, 50% resto).
        public static float ApplyTypeScaling(float rdBase, bool isPhysical)
        {
            return isPhysical ? rdBase : rdBase * 0.5f;
        }

        /// Max DEX real usando, en orden:
        /// 1) limitador ya presente en el AC (si existe);
        /// 2) armor.DexBonusLimeterAC;
        /// 3) Rulebook.Trigger(new RuleCalculateArmorMaxDexBonusLimit(wielder, armor)).
        /// Si no puede calcularse: 0.
        public static int GetArmorMaxDex(ModifiableValueArmorClass ac, ItemEntityArmor armor)
        {
            // Normaliza: negativos -> sin límite (usamos 99 para que luego el clamp 0..8 lo deje en 8)
            static int Normalize(int? v)
            {
                if (!v.HasValue) return int.MinValue; // sentinel "no value"
                return v.Value < 0 ? 99 : v.Value;
            }

            // 1) Del ítem (recalculado por RecalculateMaxDexBonus si ya pasó)
            try
            {
                var v = Normalize(armor?.DexBonusLimeterAC?.Value); // OJO: si tu API real usa 'Limiter' corrige el nombre aquí
                if (v != int.MinValue) return v;
            }
            catch { /* ignore */ }

            // 2) Del propio AC (limiters activos este frame)
            try
            {
                var lims = ac?.m_BaseAttributeBonusLimiters; // publicized
                if (lims != null)
                {
                    int best = int.MinValue;
                    foreach (var it in lims)
                    {
                        // Cuentan armadura y escudo; quédate con el más restrictivo (mínimo)
                        if (it.Source == ModifiableValueArmorClass.DexBonusLimiter.SourceType.Armor ||
                            it.Source == ModifiableValueArmorClass.DexBonusLimiter.SourceType.Shield)
                        {
                            int nv = Normalize(it.Value);
                            if (nv != int.MinValue)
                                best = (best == int.MinValue) ? nv : Math.Min(best, nv);
                        }
                    }
                    if (best != int.MinValue) return best;
                }
            }
            catch { /* ignore */ }

            // 3) Calcular mediante regla
            try
            {
                var wielder = armor?.Wielder;
                if (wielder?.Unit != null)
                {
                    var rule = Rulebook.Trigger(new RuleCalculateArmorMaxDexBonusLimit(wielder.Unit, armor));
                    if (rule.Result.HasValue)
                        return Normalize(rule.Result.Value);
                }
            }
            catch { /* ignore */ }

            // 4) Fallback: del blueprint (tipo de armadura)
            try
            {
                var bp = armor?.Blueprint as BlueprintItemArmor;
                if (bp?.Type != null)
                {
                    // La mayoría de tipos tienen MaxDexBonus
                    // Si por alguna razón es negativo o no existe, lo tratamos como sin límite
                    var typeMax = bp.Type.MaxDexterityBonus; // si tu versión es otra propiedad, usa esa
                    return typeMax < 0 ? 99 : typeMax;
                }
            }
            catch { /* ignore */ }

            // Si todo falla: sin límite
            return 99;
        }


        // Wrapper para compatibilidad cuando no tienes el AC a mano
        public static int GetArmorMaxDex(ItemEntityArmor armor)
        {
            return GetArmorMaxDex(null, armor);
        }

        // Penalización % desde Max DEX (cap 0..27), mantenemos esta tal cual
        public static int ComputeAcPenaltyPercentFromMaxDex(int maxDex)
        {
            int p = 27 - 3 * maxDex;
            if (p < 0) p = 0;
            if (p > 27) p = 27;
            return p;
        }

        // ============ UI helpers (opcionales) ============
        // Para mostrar RD de UI a partir del "base real".
        public static int ComputeArmorDrDisplayPercent(ItemEntityArmor armor)
        {
            var bp = armor?.Blueprint as BlueprintItemArmor;
            int baseReal = (bp?.Type != null) ? bp.Type.ArmorBonus : 0;
            return Math.Max(0, baseReal * 5);
        }

        // ¿Es daño físico?
        public static bool IsPhysical(DamageValue dv) => dv.Source is PhysicalDamage;
    }
}
