using System;
using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using UnityEngine; // Debug.Log

namespace CombatOverhaul.Testing
{
    /// QuickCall:
    ///   CombatOverhaul.Testing.SRProbe.Run
    ///
    /// Objetivo: Main Character
    /// C# 7.3 friendly
    internal static class SRProbe
    {
        private const string TAG = "[CO][SRMini] ";

        public static void Run()
        {
            try
            {
                var unit = TryGetMainCharacter();
                if (unit == null) { Log("No MainCharacter."); return; }

                Log("===== SR PROBE (mini) START =====");
                LogUnitHeader(unit);

                var part = unit.Descriptor != null ? unit.Descriptor.Get<UnitPartSpellResistance>() : null;
                if (part == null)
                {
                    Log("UnitPartSpellResistance = <null>");
                    LogFactsHint(unit); // snapshot útil
                    Log("===== SR PROBE (mini) END =====");
                    return;
                }

                // 1) Estado general SR
                Log("AllSRPenalty=" + part.AllSRPenalty + "  Bonuses.Count=" + (part.Bonuses != null ? part.Bonuses.Count : 0)
                    + "  IgnoreImmunities=" + part.IgnoreSpellDescriptorImmunities);

                // 2) Entradas SR crudas
                var srs = part.SRs != null ? part.SRs : new List<UnitPartSpellResistance.SpellResistanceValue>();
                Log("SRs.Count=" + srs.Count);
                for (int i = 0; i < srs.Count; i++)
                {
                    var sr = srs[i];
                    Log("  SR[" + i + "]: Value=" + sr.Value
                        + "  FactId=" + Safe(sr.FactId)
                        + "  Align=" + (sr.Alignment.HasValue ? sr.Alignment.Value.ToString() : "null")
                        + "  Desc=" + (sr.SpellDescriptor.HasValue ? sr.SpellDescriptor.Value.ToString() : "null")
                        + "  School=" + (sr.SpellSchool.HasValue ? sr.SpellSchool.Value.ToString() : "null"));
                }

                // 3) Valor “raw max” sin contexto (no aplica piercing, bonuses dict, etc.)
                int rawMax = -1;
                try { rawMax = part.GetValue((BlueprintAbility)null, (UnitEntityData)null); } catch { }
                Log("GetValue(ability:null,caster:null) [raw max] = " + rawMax);

                // 4) Resolver posibles fuentes por FactId
                TryResolveSRSources(unit, part);

                Log("====== SR PROBE (mini) END ======");
            }
            catch (Exception ex)
            {
                Log("EX: " + ex);
            }
        }

        // ============== helpers ==============

        private static UnitEntityData TryGetMainCharacter()
        {
            try { return Game.Instance != null && Game.Instance.Player != null ? Game.Instance.Player.MainCharacter : null; }
            catch { return null; }
        }

        private static void LogUnitHeader(UnitEntityData unit)
        {
            try
            {
                string sizeStr = "<unk>";
                try { sizeStr = unit.Descriptor != null && unit.Descriptor.State != null ? unit.Descriptor.State.Size.ToString() : "<unk>"; } catch { }
                if (sizeStr == "<unk>") { try { sizeStr = unit.Blueprint != null ? unit.Blueprint.Size.ToString() : "<unk>"; } catch { } }

                int lvl = 0; try { lvl = unit.Descriptor != null && unit.Descriptor.Progression != null ? unit.Descriptor.Progression.CharacterLevel : 0; } catch { }
                int myth = 0; try { myth = unit.Descriptor != null && unit.Descriptor.Progression != null ? unit.Descriptor.Progression.MythicLevel : 0; } catch { }
                string crStr = "?"; try { crStr = unit.Blueprint != null ? unit.Blueprint.CR.ToString() : "?"; } catch { }

                Log("Unit: " + Safe(unit.CharacterName) + "  [" + unit.UniqueId + "]");
                Log("Lvl=" + lvl + "  Mythic=" + myth + "  CR=" + crStr + "  Size=" + sizeStr);
            }
            catch { }
        }

        private static void TryResolveSRSources(UnitEntityData unit, UnitPartSpellResistance part)
        {
            try
            {
                if (unit == null || unit.Descriptor == null || unit.Descriptor.Facts == null) return;

                // En tu build, m_Facts es List<Kingmaker.EntitySystem.EntityFact>
                var facts = unit.Descriptor.Facts.m_Facts as List<EntityFact>;
                if (facts == null || part.SRs == null) return;

                bool any = false;
                for (int s = 0; s < part.SRs.Count; s++)
                {
                    var sr = part.SRs[s];
                    string fid = sr != null ? sr.FactId : null;
                    if (string.IsNullOrEmpty(fid)) continue;

                    bool matched = false;
                    for (int i = 0; i < facts.Count; i++)
                    {
                        var f = facts[i];
                        if (f == null) continue;

                        // Intento 1: UniqueId (propiedad pública get)
                        try
                        {
                            var uid = f.UniqueId;
                            if (!string.IsNullOrEmpty(uid) && string.Equals(uid, fid, StringComparison.OrdinalIgnoreCase))
                            {
                                LogSource(sr, f, "UniqueId");
                                matched = true; any = true; break;
                            }
                        }
                        catch { }

                        // Intento 2: sufijo del AssetGuid del blueprint del fact
                        try
                        {
                            var bp = f.Blueprint; // BlueprintFact
                            var guid = bp != null ? bp.AssetGuid.ToString() : null;
                            if (!string.IsNullOrEmpty(guid) && guid.EndsWith(fid, StringComparison.OrdinalIgnoreCase))
                            {
                                LogSource(sr, f, "AssetGuid suffix");
                                matched = true; any = true; break;
                            }
                        }
                        catch { }
                    }

                    if (!matched)
                    {
                        any = true;
                        Log("SR Value=" + sr.Value + " FactId=" + fid + " -> origen NO resuelto.");
                    }
                }

                if (!any) Log("No SR sources found to resolve.");
            }
            catch (Exception ex)
            {
                Log("ResolveSources EX: " + ex.Message);
            }
        }

        private static void LogSource(UnitPartSpellResistance.SpellResistanceValue sr, EntityFact f, string via)
        {
            // BlueprintFact no tiene 'Name'; usa 'name' (Unity) o NameSafe() si está disponible en tu build.
            BlueprintFact bp = f != null ? f.Blueprint : null;
            string name = "<no-bp>";
            try
            {
                if (bp != null) name = bp.name; // UnityEngine.Object.name
            }
            catch { }

            string guid = "<no-guid>";
            try
            {
                if (bp != null) guid = bp.AssetGuid.ToString();
            }
            catch { }

            Log("SR Value=" + sr.Value + " FactId=" + sr.FactId + "  -> " + name + " [" + guid + "]  (" + via + ")");
        }

        private static void LogFactsHint(UnitEntityData unit)
        {
            try
            {
                if (unit == null || unit.Descriptor == null || unit.Descriptor.Facts == null) return;
                var facts = unit.Descriptor.Facts.m_Facts as List<EntityFact>;
                if (facts == null) return;

                Log("--- Facts snapshot (name [guid]) ---");
                int count = 0;
                for (int i = 0; i < facts.Count && count < 25; i++)
                {
                    var f = facts[i];
                    if (f == null) continue;
                    var bp = f.Blueprint;
                    string n = bp != null ? bp.name : "<no-bp>";
                    string g = bp != null ? bp.AssetGuid.ToString() : "<no-guid>";
                    Log("  " + n + " [" + g + "]");
                    count++;
                }
            }
            catch { }
        }

        private static string Safe(string s) { return s ?? "<null>"; }
        private static void Log(string msg) { try { Debug.Log(TAG + msg); } catch { } }
    }
}
