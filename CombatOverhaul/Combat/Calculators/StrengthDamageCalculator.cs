/*using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;          // DiceType
using Kingmaker.RuleSystem.Rules.Damage;
using System;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class StrengthDamageCalculator
    {
        /// <summary>
        /// Media del dado base del arma (sin Fuerza ni críticos), sin reflexión.
        /// </summary>
        public static float GetWeaponBaseDiceAverage(ItemEntityWeapon weapon)
        {
            var bp = weapon?.Blueprint as BlueprintItemWeapon;
            if (bp == null) return 0f;

            int rolls = bp.BaseDamage.m_Rolls;     // nº de dados
            int sides = (int)bp.BaseDamage.m_Dice; // caras del dado (DiceType ya es el nº)

            if (rolls <= 0 || sides <= 0) return 0f;

            // media de un dado = (1 + N) / 2
            return rolls * (1 + sides) * 0.5f;
        }

        /// <summary>
        /// Aplica bonus de Fuerza (ajusta aquí tus reglas si no usas d20 clásico).
        /// </summary>
        public static int ApplyStrengthBonus(int baseDamage, int strMod, bool twoHanded, bool offHand)
        {
            float mult = twoHanded ? 1.5f : (offHand ? 0.5f : 1f);
            int add = (int)Math.Floor(strMod * mult);
            int total = baseDamage + add;
            return total < 0 ? 0 : total;
        }

        public static bool IsPhysical(DamageValue dv) => dv.Source is PhysicalDamage;
        /// <summary>
        /// Factor de daño por cada punto de modificador de STR para ese arma.
        /// Devuelve: 1.5f (2M), 1.0f (principal 1M), 0.5f (secundaria). Ranged => 0.
        /// </summary>
        public static float ComputeOverrideDamagePerPoint(UnitEntityData actor, ItemEntityWeapon weapon)
        {
            if (actor == null || weapon == null) return 0f;

            var bp = weapon.Blueprint;
            if (bp == null) return 0f;

            // Solo aplicamos STR a melee (ajusta si quieres incluir arcos compuestos)
            bool isRanged = bp.IsRanged;
            if (isRanged) return 0f;

            var body = actor.Body;
            bool isOffHand = body?.SecondaryHand?.MaybeWeapon == weapon;
            bool isPrimary = body?.PrimaryHand?.MaybeWeapon == weapon;
            bool isTwoHanded = bp.IsTwoHanded && isPrimary; // 2M solo cuenta si la llevas en mano principal

            if (isTwoHanded) return 1.5f;
            if (isOffHand) return 0.5f;
            if (isPrimary) return 1.0f;

            // Si no está en manos (casos raros), usa 1.0f por defecto
            return 1.0f;
        }
    }
}*/
