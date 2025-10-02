using CombatOverhaul.Combat.Opposed;
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
    [HarmonyPatch(typeof(AttackLogMessage), nameof(AttackLogMessage.GetData))]
    internal static class Patch_AttackLogMessage_GetData
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            var tooltipCtor = AccessTools.Constructor(typeof(TooltipTemplateCombatLogMessage),
                new[] { typeof(string), typeof(string) });

            var buildBody = AccessTools.Method(typeof(Patch_AttackLogMessage_GetData),
                nameof(BuildBodyWithCustomHeader));

            for (int i = 0; i < codes.Count; i++)
            {
                var instr = codes[i];
                if (instr.opcode == OpCodes.Newobj && Equals(instr.operand as ConstructorInfo, tooltipCtor))
                {
                    // Pila justo antes del newobj: ..., [ldloc header(text)], [ldloc body(text2)], newobj
                    int bodyIdx = i - 1;
                    if (bodyIdx >= 0)
                    {
                        // Insertamos: ldarg.0 (this), ldarg.1 (rule) antes del ldloc body
                        codes.Insert(bodyIdx, new CodeInstruction(OpCodes.Ldarg_0)); // this
                        codes.Insert(bodyIdx + 1, new CodeInstruction(OpCodes.Ldarg_1)); // rule
                        // Tras el (ahora) ldloc body, llamamos a nuestro builder
                        codes.Insert(bodyIdx + 3, new CodeInstruction(OpCodes.Call, buildBody));

                        i += 3; // saltar nuestras inserciones
                    }
                }
            }

            return codes;
        }

        /// <summary>
        /// Prependemos el header custom, quitamos el bloque vanilla y normalizamos a
        /// EXACTAMENTE una línea en blanco entre nuestro bloque y "Attack Bonus".
        /// </summary>
        public static string BuildBodyWithCustomHeader(AttackLogMessage self, RuleAttackRoll rule, string originalBody)
        {
            if (rule == null) return originalBody ?? string.Empty;
            if (originalBody == null) originalBody = string.Empty;

            int roll = rule.D20;
            int atkBonus = rule.AttackBonus;
            int ac = rule.TargetAC;

            // === Usa tu modelo de oposición ===
            var res = OpposedRollCore.ResolveForAttack(atkBonus, ac, roll);
            int pct = (int)Math.Round(res.p5 * 100.0f); // p5 ya viene granular (5%)
            int needed = res.TN;                            // clamp 2..20 según tu modelo

            // Resultado mostrado: usa el real del juego (regla ya resuelta) o, si prefieres, res.success
            string resultText = rule.IsHit ? "hit" : roll == 1 ? "critical miss" : "miss";

            string customHeader =
                "Attack roll: " + roll + "\n" +
                "Chance of hit: " + pct + "% (" + needed + ")\n" +
                "Result: " + resultText;

            // Elimina el bloque vanilla de 3 líneas
            var rxBlock = new Regex(
                @"(?im)^(?:\s*)Attack result:.*\r?\n^Target'?s Armor Class:.*\r?\n^Result:.*\r?\n?",
                RegexOptions.Multiline);
            string body = rxBlock.Replace(originalBody, string.Empty);

            // Limpia whitespace inicial y deja DOS saltos entre tu bloque y el breakdown
            body = Regex.Replace(body, @"^\s+", "");

            return customHeader + "\n\n" + body;
        }

    }
}
