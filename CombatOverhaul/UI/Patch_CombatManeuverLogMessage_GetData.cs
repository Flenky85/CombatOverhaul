using CombatOverhaul.Calculators;
using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace CombatOverhaul.UI
{
    [HarmonyPatch(typeof(CombatManeuverLogMessage), nameof(CombatManeuverLogMessage.GetData))]
    internal static class Patch_CombatManeuverLogMessage_GetData
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var tooltipCtor = AccessTools.Constructor(typeof(TooltipTemplateCombatLogMessage),
                new[] { typeof(string), typeof(string) });

            var buildBody = AccessTools.Method(typeof(Patch_CombatManeuverLogMessage_GetData),
                nameof(BuildBodyWithCustomHeader));

            for (int i = 0; i < codes.Count; i++)
            {
                var instr = codes[i];
                if (instr.opcode == OpCodes.Newobj && Equals(instr.operand as ConstructorInfo, tooltipCtor))
                {
                    // ... [ldloc header], [ldloc body], newobj
                    int bodyIdx = i - 1;
                    if (bodyIdx >= 0)
                    {
                        // Insertamos this y rule antes del ldloc body
                        codes.Insert(bodyIdx, new CodeInstruction(OpCodes.Ldarg_0)); // this
                        codes.Insert(bodyIdx + 1, new CodeInstruction(OpCodes.Ldarg_1)); // rule
                        // Llamamos a nuestro builder después del ldloc body
                        codes.Insert(bodyIdx + 3, new CodeInstruction(OpCodes.Call, buildBody));
                        i += 3;
                    }
                }
            }
            return codes;
        }

        /// Construye cabecera custom de maniobras y limpia el bloque vanilla.
        public static string BuildBodyWithCustomHeader(RuleCombatManeuver rule, string originalBody)
        {
            if (rule == null) return originalBody ?? string.Empty;
            originalBody ??= string.Empty;

            // A, D, d20 ya calculados por OnTrigger
            int roll = rule.InitiatorRoll;
            int A = rule.InitiatorCMB;
            int D = rule.TargetCMD;

            var res = OpposedRollCore.ResolveD20(A, D, roll);
            int pct = (int)Math.Round(res.P5 * 100.0f);
            int needed = res.TN;

            string resultText = rule.Result switch
            {
                CombatManeuverResult.CriticalSuccess => "critical success",
                CombatManeuverResult.CriticalFail => "critical fail",
                CombatManeuverResult.Success => "success",
                _ => "fail"
            };

            // Bloque custom (2 líneas, sin confirmación)
            string custom =
                "Maneuver roll: " + roll + "\n" +
                "Chance of success: " + pct + "% (" + needed + ")\n" +
                "Result: " + resultText;

            // Quitar bloque vanilla inicial de maniobras
            // "Attack result: …" + "Target's CMD: …" + "Result: …"
            var rxBlock = new Regex(
                @"(?im)^\s*Attack result:.*\r?\n^\s*Target'?s CMD:.*(?:\r?\n^\s*Result:.*)?\r?\n?",
                RegexOptions.Multiline);

            string body = rxBlock.Replace(originalBody, string.Empty);

            // Limpia whitespace inicial y dobles saltos sobrantes
            body = Regex.Replace(body, @"^\s+", "");
            body = Regex.Replace(body, @"\n{3,}", "\n\n");

            // Ensamblado final: nuestro bloque + doble salto + resto (breakdowns, etc.)
            return custom + "\n\n" + body;
        }
    }
}
