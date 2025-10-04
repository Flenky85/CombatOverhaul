using System;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class MagicSrCalc
    {
        /// Con contexto:
        ///   - Si inmune -> int.MaxValue.
        ///   - Devuelve déficit = clamp(SR_efectiva - Penetration, 0..100) (sin d20).
        /// Sin contexto:
        ///   - Devuelve SR cruda (0..100) contra el initiator (si se pasa), o cruda genérica.
        internal static int ComputeSrDeficitNoRoll(
            MechanicsContext context,
            UnitEntityData target,
            UnitEntityData initiatorForRaw = null)
        {
            if (target == null) return 0;

            var srPart = target.Get<UnitPartSpellResistance>();
            if (srPart == null) return 0;

            // SIN CONTEXTO: SR cruda razonable
            if (context == null)
            {
                int srRaw = 0;
                try
                {
                    srRaw = srPart.GetValue((Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbility)null, initiatorForRaw);
                }
                catch { /* fallback 0 */ }
                return Clamp01_100(srRaw);
            }

            // CON CONTEXTO
            try
            {
                if (srPart.IsImmune(context, false))
                    return int.MaxValue;
            }
            catch
            {
                // Si falla la comprobación de inmunidad, seguimos como no inmune.
            }

            int srEff = 0;
            try
            {
                srEff = srPart.GetValue(context);
            }
            catch { /* fallback 0 */ }
            srEff = Clamp01_100(srEff);

            int casterLevel = 0;
            try { casterLevel = context.Params?.CasterLevel ?? 0; } catch { /* 0 */ }

            int penetration = casterLevel;

            // Reemplazo de chequeo por stat/progresión (si existe)
            var caster = context.MaybeCaster;
            var replace = caster?.Get<UnitPartSpellResistanceCheckReplace>();
            if (replace != null && caster != null)
            {
                try
                {
                    var stat = caster.Stats?.GetStat(replace.StatType);
                    int statVl = stat?.ModifiedValue ?? 0;
                    int progVl = replace.Progression.CalculateValue(statVl);
                    if (progVl > penetration) penetration = progVl;
                }
                catch { /* ignorar: dejamos penetration tal cual */ }
            }

            int deficit = srEff - penetration;
            return Clamp01_100(deficit);
        }

        private static int Clamp01_100(int v)
        {
            if (v < 0) return 0;
            if (v > 100) return 100;
            return v;
        }
    }
}
