using CombatOverhaul.Combat.Opposed; // si no lo usas aquí, puedes quitarlo
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
    [HarmonyPatch(typeof(SavingThrowMessage), nameof(SavingThrowMessage.GetData))]
    internal static class Patch_SavingThrowMessage_GetData
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var tooltipCtor = AccessTools.Constructor(typeof(TooltipTemplateCombatLogMessage),
                new[] { typeof(string), typeof(string) });

            var rewriter = AccessTools.Method(typeof(Patch_SavingThrowMessage_GetData),
                nameof(ReplaceSavingThrowLineOnly));

            for (int i = 0; i < codes.Count; i++)
            {
                var ins = codes[i];
                if (ins.opcode == OpCodes.Newobj && Equals(ins.operand as ConstructorInfo, tooltipCtor))
                {
                    // stack: ..., [ldloc message], [ldloc body], newobj
                    int bodyIdx = i - 1;
                    if (bodyIdx >= 0)
                    {
                        codes.Insert(bodyIdx, new CodeInstruction(OpCodes.Ldarg_0)); // this
                        codes.Insert(bodyIdx + 1, new CodeInstruction(OpCodes.Ldarg_1)); // rule
                        codes.Insert(bodyIdx + 3, new CodeInstruction(OpCodes.Call, rewriter));
                        i += 3;
                    }
                }
            }
            return codes;
        }

        /// Reemplaza SOLO la línea "Saving throw result: …" por "Saving throw: <d20>".
        /// Mantiene intacta "Difficulty (DC): …" y todo lo demás.
        public static string ReplaceSavingThrowLineOnly(SavingThrowMessage self, RuleSavingThrow rule, string originalBody)
        {
            if (string.IsNullOrEmpty(originalBody) || rule == null) return originalBody ?? string.Empty;

            int roll = rule.D20;

            // 1) Reemplaza "Saving throw result: …"
            var rxSaving = new Regex(@"(?im)^\s*Saving throw result:.*$", RegexOptions.Multiline);
            string body = rxSaving.Replace(originalBody, $"Saving throw: {roll}", 1);

            // 2) Calcula % y TN (siguiendo el criterio vanilla para successBonus mostrado)
            int sucBonusUsed = rule.RequiresSuccessBonus ? rule.SuccessBonus : 0;
            int A = rule.StatValue + sucBonusUsed;
            int D = rule.DifficultyClass;

            var res = OpposedRollCore.ResolveD20(A, D, roll);
            int pct = (int)Math.Round(res.P5 * 100.0f);
            int tn = res.TN;

            // 3) Captura y elimina la línea original de DC
            var rxDC = new Regex(@"(?im)^\s*Difficulty\s*\(DC\):\s*\d+.*\r?\n?", RegexOptions.Multiline);
            string dcLine = "Difficulty (DC): " + D; // fallback
            var mDC = rxDC.Match(body);
            if (mDC.Success) dcLine = mDC.Value.TrimEnd('\r', '\n');
            body = rxDC.Replace(body, "", 1);

            // 4) Reescribe la línea "Result: …" para insertar:
            //    Chance of success POR ENCIMA y Difficulty (DC) DEBAJO con un salto en blanco
            var rxResult = new Regex(@"(?im)^(?<indent>\s*)Result:\s*.*$", RegexOptions.Multiline);
            body = rxResult.Replace(
                body,
                m =>
                {
                    var indent = m.Groups["indent"].Value;
                    return $"{indent}Chance of success: {pct}% ({tn})\n{m.Value}\n\n{dcLine}";
                },
                1
            );

            return body;
        }



    }
}
