using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings.GameLog; // AttackLogMessage
using Kingmaker.RuleSystem.Rules;                // RuleAttackRoll
using CombatOverhaul.Combat.Opposed;

namespace CombatOverhaul.Patches.Attack
{
    /// Inserta " [TN X | Y%]" en la MISMA línea del mensaje de ataque,
    /// justo antes de hacer sb.ToString(), duplicando la instancia de StringBuilder en la pila.
    [HarmonyPatch(typeof(AttackLogMessage), nameof(AttackLogMessage.GetData))]
    internal static class Patch_AttackLogMessage_GetData
    {
        // Llama OpposedRollStore y añade el sufijo al StringBuilder
        private static void AppendTNToLine(StringBuilder sb, RuleAttackRoll rule)
        {
            if (sb == null || rule == null) return;
            if (!OpposedRollStore.TryGet(rule, out var res)) return;
            sb.Append($" [TN {res.TN} | {(res.p5 * 100f):0}%]");
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = new List<CodeInstruction>(instructions);

            // Buscamos la llamada al StringBuilder.ToString() (sin parámetros)
            var sbToString = AccessTools.Method(typeof(StringBuilder), nameof(StringBuilder.ToString), new System.Type[] { });
            var helper = AccessTools.Method(typeof(Patch_AttackLogMessage_GetData), nameof(AppendTNToLine));

            for (int i = 0; i < code.Count; i++)
            {
                if (code[i].Calls(sbToString))
                {
                    // En IL, justo antes del ToString() hay un ldloc (u otra carga) que deja 'sb' en la pila.
                    // Insertamos:
                    //   dup           ; duplica la instancia de sb
                    //   ldarg.1       ; 'rule' de GetData(RuleAttackRoll rule)
                    //   call helper   ; AppendTNToLine(sb, rule) -> consume una 'sb', deja la otra en pila
                    var injected = new List<CodeInstruction>
                    {
                        new CodeInstruction(OpCodes.Dup),
                        new CodeInstruction(OpCodes.Ldarg_1),
                        new CodeInstruction(OpCodes.Call, helper)
                    };

                    code.InsertRange(i, injected);
                    break; // solo antes del primer ToString() que construye 'text'
                }
            }

            return code;
        }
    }
}
