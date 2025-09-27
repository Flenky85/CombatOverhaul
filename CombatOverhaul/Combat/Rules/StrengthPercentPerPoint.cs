using System;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;               // DamageType
using UnityEngine;

namespace CombatOverhaul.Combat.Rules
{
    /// Aplica tu % de daño por punto de STR **solo** al daño FÍSICO (B/P/S)
    /// usando BonusPercent (para que NO salga "Vulnerabilidad").
    /// - 1 arma: 30% por punto
    /// - Dual: Primary 20% por punto, Offhand 20% por punto
    /// - Naturales sin arma: % por ataque según tabla (total ~30%)
    /// - Naturales con arma: tabla como si n+=2 (empieza en 3)
    internal sealed class StrengthPercentPerPoint :
        IGlobalRulebookHandler<RuleCalculateDamage>,
        ISubscriber, IGlobalSubscriber
    {
        // % por ataque natural (por punto de STR). Índice = nº ataques (clamp 1..10)
        private static readonly float[] NaturalPct = {
            0.00f, // 0 (no se usa)
            0.30f, // 1
            0.15f, // 2
            0.10f, // 3
            0.075f,// 4
            0.06f, // 5
            0.05f, // 6
            0.0429f,//7
            0.0375f,//8
            0.0333f,//9
            0.03f   // 10+
        };

        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            try
            {
                var attacker = evt?.Initiator;
                var attackRoll = evt?.ParentRule?.AttackRoll;
                if (attacker == null || attackRoll == null) return;

                int strMod = attacker.Stats?.Strength?.Bonus ?? 0;
                if (strMod == 0) return; // si quieres ignorar STR negativa: if (strMod <= 0) return;

                var weapon = attackRoll.Weapon;
                bool hasWeapon = weapon != null;
                bool isNaturalAttack = hasWeapon && weapon.Blueprint != null && weapon.Blueprint.IsNatural && !weapon.Blueprint.IsUnarmed;
                bool isManufacturedAttack = hasWeapon && !isNaturalAttack; // unarmed cuenta como manufacturada

                // Conteos de equipo para clasificar el golpe
                var body = attacker.Body;
                bool hasPrimaryManuf = body?.PrimaryHand?.MaybeWeapon != null && !(body.PrimaryHand.MaybeWeapon.Blueprint?.IsNatural ?? false);
                bool hasOffManuf = body?.SecondaryHand?.MaybeWeapon != null && !(body.SecondaryHand.MaybeWeapon.Blueprint?.IsNatural ?? false);
                bool anyManufEquipped = hasPrimaryManuf || hasOffManuf;

                bool isOffhandAttack = hasWeapon && (body?.SecondaryHand?.MaybeWeapon == weapon);
                // bool isPrimaryAttack = hasWeapon && (body?.PrimaryHand?.MaybeWeapon == weapon);

                // % por punto según tus reglas
                float perPoint = 0f;
                if (isManufacturedAttack)
                {
                    if (hasPrimaryManuf && !hasOffManuf) perPoint = 0.30f;          // 1 arma
                    else if (hasPrimaryManuf && hasOffManuf) perPoint = 0.20f;      // dual: 20% tanto primaria como off
                    else perPoint = 0.30f;                                          // caso raro → 1 arma
                }
                else if (isNaturalAttack)
                {
                    int naturals = CountNaturalWeapons(attacker);
                    if (naturals <= 0) naturals = 1;
                    naturals = anyManufEquipped ? Math.Min(naturals + 2, 10) : Math.Min(naturals, 10);
                    perPoint = NaturalPct[naturals];
                }
                else
                {
                    return; // sin arma y no natural
                }

                if (perPoint == 0f) return;

                // Extra en porcentaje total por ataque: STRmod * perPoint
                // Lo aplicamos SOLO a daños físicos con BonusPercent (NO vulnerabilidad)
                int extraPercent = (int)Math.Round(strMod * perPoint * 100f);

                foreach (var d in evt.ParentRule.DamageBundle)
                {
                    if (d == null) continue;
                    if (d.Type != DamageType.Physical) continue; // sólo B/P/S
                    // Si NO quieres afectar precisión, descomenta:
                    // if (d.Precision) continue;

                    d.BonusPercent += extraPercent;
                }

#if DEBUG
                Debug.Log($"[CO][STR%→Phys+Bonus%] {attacker.CharacterName} STRmod={strMod} perPoint={(perPoint*100):0.#}% extra={extraPercent}% " +
                          $"{(isManufacturedAttack ? (isOffhandAttack ? "Offhand" : "Primary/Single") : "Natural")}");
#endif
            }
            catch (Exception ex)
            {
                Debug.LogError("[CombatOverhaul][STR%→Phys+Bonus%] " + ex);
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt) { }

        private static int CountNaturalWeapons(UnitEntityData unit)
        {
            if (unit?.Body == null) return 0;
            int count = 0;

            TryCount(unit.Body.PrimaryHand, ref count);
            TryCount(unit.Body.SecondaryHand, ref count);
            if (unit.Body.AdditionalLimbs != null)
                foreach (var slot in unit.Body.AdditionalLimbs) TryCount(slot, ref count);
            return count;

            static void TryCount(WeaponSlot slot, ref int c)
            {
                var w = slot?.MaybeWeapon;
                var bp = w?.Blueprint;
                if (bp != null && bp.IsNatural && !bp.IsUnarmed) c++;
            }
        }
    }
}
