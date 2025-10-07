using CombatOverhaul.Calculators;
using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.Localization;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.Common;
using Kingmaker.UI.Models.Log;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Base;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.Utility;
using System;
using UnityEngine;

namespace CombatOverhaul.Patches.UI.Roll
{
    [HarmonyPatch(typeof(BaseRollSkillCheckLogThread), "LogRuleSkillCheck")]
    internal static class SkillCheck_LogThread
    {
        static bool Prefix(BaseRollSkillCheckLogThread __instance, RuleSkillCheck check)
        {
            var unit = check?.Initiator;
            if (unit == null || unit.CombatState == null || !unit.CombatState.IsInCombat)
                return true;

            try
            {
                using (ProfileScope.New("Build Skill Check Log Message [CO]", (UnityEngine.Object)null))
                using (GameLogContext.Scope)
                {
                    GameLogContext.SourceUnit = check.Initiator;
                    GameLogContext.Text = UIUtility.GetStatText(check.StatType);
                    GameLogContext.Roll = check.RollResult;
                    GameLogContext.D20 = check.D20;
                    GameLogContext.Modifier = check.TotalBonus;
                    GameLogContext.DC = check.NonNegativeDC;

                    int successBonus = check.RequiresSuccessBonus ? check.SuccessBonus : 0;
                    int roll = check.D20;
                    int A = check.TotalBonus + successBonus;
                    int D = check.DifficultyClass; 

                    var res = OpposedRollCore.ResolveD20(A, D, roll);
                    int pct = (int)Math.Round(res.P5 * 100.0f);
                    int tn = res.TN;

                    bool passed = check.RollResult >= D;

                    GameLogMessage tmpl = passed
                        ? LogThreadBase.Strings.SkillCheckSuccess
                        : LogThreadBase.Strings.SkillCheckFail;

                    LocalizedString headerLS = tmpl?.Message;
                    string headerStr = headerLS?.ToString();
                    if (string.IsNullOrEmpty(headerStr))
                        headerStr = passed ? "Skill check success" : "Skill check failed";
                    Color32 color = tmpl?.GetColor() ?? GameLogStrings.Instance.DefaultColor;

                    // Cuerpo EXACTO como en ST
                    var sb = GameLogUtility.StringBuilder;
                    sb.Append("Skill check: ").Append(roll).AppendLine();
                    sb.Append("Chance of success: ").Append(pct).Append("% (").Append(tn).Append(')').AppendLine();
                    sb.Append("Result: ").Append(passed ? "success" : "fail").AppendLine();

                    string bodyText = sb.ToString();
                    sb.Clear();

                    TooltipTemplateCombatLogMessage template = null;
                    if (!string.IsNullOrEmpty(bodyText))
                        template = new TooltipTemplateCombatLogMessage(headerStr, bodyText);

                    var icon = GameLogContext.GetIcon();
                    var message = new CombatLogMessage(headerStr, color, icon, template, true);
                    
                    var addMsg = AccessTools.Method(typeof(LogThreadBase), "AddMessage", new Type[] { typeof(CombatLogMessage) });
                    addMsg.Invoke(__instance, new object[] { message });

                    return false; 
                }
            }
            finally
            {
                StatModifiersBreakdown.Clear();
            }
        }
    }
}
