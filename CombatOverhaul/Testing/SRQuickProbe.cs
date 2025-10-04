using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine; // Debug.Log

namespace CombatOverhaul.Testing
{
    /// QuickCall:
    ///   Llama a: CombatOverhaul.Testing.SRQuickProbe.Run()
    /// Objetivo por defecto: Main Character
    internal static class SRQuickProbe
    {
        private const string TAG = "[CO][SR] ";

        /// Punto de entrada para QuickCall.
        public static void Run()
        {
            try
            {
                var unit = TryGetMainCharacter();
                if (unit == null)
                {
                    Log("No se encontró MainCharacter. Aborta.");
                    return;
                }

                Log("======== SR PROBE START ========");
                BasicUnitDump(unit);

                var part = unit.Descriptor != null ? unit.Descriptor.Get<UnitPartSpellResistance>() : null;
                if (part == null)
                {
                    Log("UnitPartSpellResistance = <null>. Esta unidad no tiene SR activa.");
                    Log("========= SR PROBE END =========");
                    return;
                }

                DumpSRPart(unit, part);

                // Prueba adicional opcional: evaluar SR “como si” hubiera contexto (mínimo)
                // Ojo: sin contexto real, solo registramos el valor base con GetValue((MechanicsContext)null)
                int valueNoCtx = SafeGetValueNoContext(part);
                Log("GetValue(context:null) = " + valueNoCtx);

                // Si quieres probar contra una habilidad concreta, pega su guid en TEST_ABILITY_GUID
                // y descomenta el bloque:
                // DumpVsAbility(unit, part, "PUT_ABILITY_GUID_HERE");

                Log("========= SR PROBE END =========");
            }
            catch (Exception ex)
            {
                Log("EXCEPCIÓN en SRQuickProbe.Run(): " + ex);
            }
        }

        // =================== helpers ===================

        private static UnitEntityData TryGetMainCharacter()
        {
            try
            {
                var player = Game.Instance != null ? Game.Instance.Player : null;
                if (player == null) return null;
                return player.MainCharacter;
            }
            catch { return null; }
        }

        private static void BasicUnitDump(UnitEntityData unit)
        {
            try
            {
                Log("Unit: " + Safe(unit.CharacterName) + "  [" + unit.UniqueId + "]");
                try
                {
                    string sizeStr = "<unk>";
                    try { sizeStr = unit.Descriptor != null && unit.Descriptor.State != null ? unit.Descriptor.State.Size.ToString() : "<unk>"; } catch { }
                    if (sizeStr == "<unk>")
                    {
                        try { sizeStr = unit.Blueprint != null ? unit.Blueprint.Size.ToString() : "<unk>"; } catch { }
                    }

                    int charLevel = 0;
                    try { charLevel = unit.Descriptor != null && unit.Descriptor.Progression != null ? unit.Descriptor.Progression.CharacterLevel : 0; } catch { }

                    int mythicLevel = 0;
                    try { mythicLevel = unit.Descriptor != null && unit.Descriptor.Progression != null ? unit.Descriptor.Progression.MythicLevel : 0; } catch { }

                    string crStr = "?";
                    try
                    {
                        // CR viene del Blueprint de la unidad
                        crStr = unit.Blueprint != null ? unit.Blueprint.CR.ToString() : "?";
                    }
                    catch { }

                    Log("Lvl: " + charLevel + "  Mythic: " + mythicLevel + "  CR: " + crStr + "  Size: " + sizeStr);
                }
                catch (Exception ex)
                {
                    Log("Basic stats dump EX: " + ex.Message);
                }
                Log("Alignment: " + unit.Descriptor.Alignment.ValueRaw);
                Log("Facts totales: " + (unit.Descriptor.Facts != null ? unit.Descriptor.Facts.m_Facts.Count : 0));
                Log("Buffs activos: " + (unit.Descriptor.Buffs != null ? unit.Descriptor.Buffs.RawFacts.Count : 0));
            }
            catch (Exception ex)
            {
                Log("BasicUnitDump EX: " + ex.Message);
            }
        }

        private static void DumpSRPart(UnitEntityData unit, UnitPartSpellResistance part)
        {
            try
            {
                Log("--- UnitPartSpellResistance ---");
                Log("AllSRPenalty: " + SafeInt(part.AllSRPenalty));
                Log("Bonuses.Count: " + (part.Bonuses != null ? part.Bonuses.Count : 0));
                if (part.Bonuses != null && part.Bonuses.Count > 0)
                {
                    foreach (var kv in part.Bonuses)
                        Log("  Bonus[id=" + kv.Key + "] = " + kv.Value);
                }

                Log("IgnoreSpellDescriptorImmunities: " + part.IgnoreSpellDescriptorImmunities);

                // SR entries
                var srs = part.SRs != null ? part.SRs : new List<UnitPartSpellResistance.SpellResistanceValue>();
                Log("SRs.Count: " + srs.Count);
                for (int i = 0; i < srs.Count; i++)
                {
                    var sr = srs[i];
                    Log("  SR[" + i + "]: Id=" + sr.Id
                        + " FactId=" + Safe(sr.FactId)
                        + " Value=" + sr.Value
                        + " Align=" + (sr.Alignment.HasValue ? sr.Alignment.Value.ToString() : "null")
                        + " Descriptor=" + (sr.SpellDescriptor.HasValue ? sr.SpellDescriptor.Value.ToString() : "null")
                        + " School=" + (sr.SpellSchool.HasValue ? sr.SpellSchool.Value.ToString() : "null"));
                }

                // Immunities
                var imms = part.Immunities != null ? part.Immunities : new List<UnitPartSpellResistance.SpellImmunity>();
                Log("Immunities.Count: " + imms.Count);
                for (int i = 0; i < imms.Count; i++)
                {
                    var im = imms[i];
                    var exCount = im.Exceptions != null ? im.Exceptions.Length : 0;
                    Log("  IMM[" + i + "]: Id=" + im.Id
                        + " Type=" + im.Type
                        + " Align=" + im.Alignment
                        + " SpellDesc=" + im.SpellDescriptor
                        + " Exceptions=" + exCount
                        + " CasterIgnoreFact=" + (im.CasterIgnoreImmunityFact != null ? im.CasterIgnoreImmunityFact.name : "null"));
                }

                // Heurística: listar buffs/facts que suelen dar SR (pista)
                TryListKnownSRSources(unit);
            }
            catch (Exception ex)
            {
                Log("DumpSRPart EX: " + ex);
            }
        }

        private static void DumpVsAbility(UnitEntityData unit, UnitPartSpellResistance part, string abilityGuid)
        {
            try
            {
                if (string.IsNullOrEmpty(abilityGuid)) return;

                var bp = TryGetAbilityByGuid(abilityGuid);
                if (bp == null)
                {
                    Log("Ability " + abilityGuid + " no encontrada.");
                    return;
                }

                int v = part.GetValue(bp, null);
                Log("GetValue(ability=" + bp.name + ", caster=null) = " + v);

                bool imm = part.IsImmune(bp, null);
                Log("IsImmune(ability=" + bp.name + ", caster=null) = " + imm);
            }
            catch (Exception ex)
            {
                Log("DumpVsAbility EX: " + ex);
            }
        }

        // ================= low-level utils =================

        private static int SafeGetValueNoContext(UnitPartSpellResistance part)
        {
            try { return part.GetValue((MechanicsContext)null); } catch { return -1; }
        }

        private static BlueprintAbility TryGetAbilityByGuid(string guid)
        {
            try
            {
                var refObj = Kingmaker.Blueprints.ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(guid);
                return refObj;
            }
            catch { return null; }
        }

        private static void TryListKnownSRSources(UnitEntityData unit)
        {
            try
            {
                // Esto no es exhaustivo; solo pistas útiles para ver de dónde puede venir SR
                var hits = new List<string>();

                // Buffs que contengan “Spell Resistance” en nombre
                if (unit.Descriptor != null && unit.Descriptor.Buffs != null)
                {
                    foreach (var f in unit.Descriptor.Buffs.RawFacts)
                    {
                        var n = f != null && f.Blueprint != null ? f.Blueprint.NameSafe() : null;
                        if (!string.IsNullOrEmpty(n) && n.IndexOf("spell resist", StringComparison.OrdinalIgnoreCase) >= 0)
                            hits.Add("BUFF: " + n);
                    }
                }

                // Hechizos/facts del unit descriptor
                if (unit.Descriptor != null && unit.Descriptor.Facts != null)
                {
                    foreach (var f in unit.Descriptor.Facts.m_Facts)
                    {
                        var bp = f != null ? f.Blueprint : null;
                        var n = bp != null ? bp.NameSafe() : null;
                        if (!string.IsNullOrEmpty(n) && n.IndexOf("spell resist", StringComparison.OrdinalIgnoreCase) >= 0)
                            hits.Add("FACT: " + n);
                    }
                }

                if (hits.Count > 0)
                {
                    Log("Posibles fuentes SR:");
                    for (int i = 0; i < hits.Count; i++) Log("  - " + hits[i]);
                }
                else
                {
                    Log("No se detectaron fuentes claras de SR por nombre (heurística).");
                }
            }
            catch (Exception ex)
            {
                Log("TryListKnownSRSources EX: " + ex.Message);
            }
        }

        private static string Safe(string s) { return s ?? "<null>"; }
        private static int SafeInt(int v) { return v; }

        private static void Log(string msg)
        {
            try { Debug.Log(TAG + msg); } catch { /* swallow */ }
        }
    }
}
