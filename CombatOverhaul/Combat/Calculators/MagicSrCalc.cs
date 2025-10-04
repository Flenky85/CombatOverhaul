using System;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.EntitySystem.Stats;
using UnityEngine; // Debug

namespace CombatOverhaul.Combat.Calculators
{
    internal static class MagicSrCalc
    {
        private const string TAG = "[CO][MagicSrCalc] ";

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
            try
            {
                if (target == null)
                {
                    Debug.Log(TAG + "target == null -> 0");
                    return 0;
                }

                var p = target.Get<UnitPartSpellResistance>();
                if (p == null)
                {
                    Debug.Log(TAG + "UnitPartSpellResistance == null -> 0");
                    return 0;
                }

                // SIN CONTEXTO: SR cruda razonable
                if (context == null)
                {
                    int srRaw;
                    try
                    {
                        srRaw = p.GetValue((Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbility)null, initiatorForRaw);
                        if (srRaw < 0) srRaw = 0; else if (srRaw > 100) srRaw = 100;
                    }
                    catch { srRaw = 0; }
                    Debug.Log(TAG + "NO CTX -> SR raw=" + srRaw + (initiatorForRaw != null ? ("  caster=" + initiatorForRaw.CharacterName) : ""));
                    return srRaw;
                }

                // CON CONTEXTO
                bool immune;
                try { immune = p.IsImmune(context, false); } catch { immune = false; }
                if (immune)
                {
                    Debug.Log(TAG + "WITH CTX -> IMMUNE");
                    return int.MaxValue;
                }

                int srEff;
                try { srEff = p.GetValue(context); } catch { srEff = 0; }
                if (srEff < 0) srEff = 0; else if (srEff > 100) srEff = 100;

                int casterLevel = 0;
                try { casterLevel = context.Params != null ? context.Params.CasterLevel : 0; } catch { }

                int penetration = casterLevel;

                var caster = context.MaybeCaster;
                var replace = caster != null ? caster.Get<UnitPartSpellResistanceCheckReplace>() : null;
                if (replace != null && caster != null)
                {
                    try
                    {
                        var stat = caster.Stats.GetStat(replace.StatType);
                        int statVl = stat != null ? stat.ModifiedValue : 0;
                        int progVl = replace.Progression.CalculateValue(statVl);
                        if (progVl > casterLevel) penetration = progVl;
                    }
                    catch { /* ignore */ }
                }

                int deficit = srEff - penetration;
                if (deficit < 0) deficit = 0; else if (deficit > 100) deficit = 100;

                Debug.Log(TAG + "WITH CTX -> srEff=" + srEff + "  pen=" + penetration + "  DEFICIT=" + deficit
                               + (caster != null ? ("  caster=" + caster.CharacterName) : ""));

                return deficit;
            }
            catch (Exception ex)
            {
                Debug.Log(TAG + "EX: " + ex);
                return 0;
            }
        }
    }
}
