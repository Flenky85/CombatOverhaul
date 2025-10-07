using CombatOverhaul.Calculators;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.Common;
using Kingmaker.UI.Models.Log;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.Utility;
using System;

namespace CombatOverhaul.Patches.UI.Roll
{
    [HarmonyPatch(typeof(SavingThrowMessage), nameof(SavingThrowMessage.GetData))]
    internal static class SavingThrowLogMessage_GetData
    {
        static bool Prefix(SavingThrowMessage __instance, RuleSavingThrow rule, ref CombatLogMessage __result)
        {
            try
            {
                using (ProfileScope.New("Build Saving Throw Log Message [CO]", (UnityEngine.Object)null))
                using (GameLogContext.Scope)
                {
                    int successBonus = rule.RequiresSuccessBonus ? rule.SuccessBonus : 0;
                    GameLogContext.SourceUnit = rule.Initiator;
                    GameLogContext.Text = UIUtility.GetStatText(rule.StatType);
                    GameLogContext.Roll = rule.RollResult;
                    GameLogContext.D20 = rule.D20;                 
                    GameLogContext.Modifier = rule.StatValue + successBonus;
                    GameLogContext.DC = rule.DifficultyClass;

                    LocalizedString headerLS = __instance.Message;
                    string headerStr = headerLS?.ToString();
                    if (string.IsNullOrEmpty(headerStr))
                    {
                        __result = null;
                        return false;
                    }

                    var stats = LocalizedTexts.Instance.Stats;
                    var sb = GameLogUtility.StringBuilder;

                    string effectName = rule.Reason?.Name;
                    string effectFmt = __instance.Effect?.ToString();
                    if (!string.IsNullOrWhiteSpace(effectName) && !string.IsNullOrEmpty(effectFmt))
                    {
                        sb.Append(string.Format(effectFmt, effectName));
                        sb.AppendLine();
                    }

                    int roll = rule.D20;
                    int A = rule.StatValue + successBonus;
                    int D = rule.DifficultyClass;

                    var res = OpposedRollCore.ResolveD20(A, D, roll);
                    int pct = (int)Math.Round(res.P5 * 100.0f);
                    int tn = res.TN;

                    bool passed = rule.RollResult >= D;

                    sb.Append("Saving throw: ").Append(roll).AppendLine();
                    sb.Append("Chance of success: ").Append(pct).Append("% (DC: ").Append(tn).Append(')').AppendLine();
                    sb.Append("Result: ").Append(passed ? "success" : "fail").AppendLine();

                    var stat = rule.Initiator.Stats.GetStat<ModifiableValue>(rule.StatType);
                    if (stat != null)
                    {
                        sb.AppendLine();

                        sb.Append("<b>")
                          .Append(stats.GetText(rule.StatType))
                          .Append(": ")
                          .Append(UIUtility.AddSign(rule.StatValue + successBonus))
                          .Append("</b>").AppendLine();

                        var tooltips = Game.Instance.BlueprintRoot.LocalizedTexts.UserInterfacesText.Tooltips;
                        sb.Append(string.Format("{0}: {1}\n", tooltips.ClassBonus, stat.BaseValue));

                        StatModifiersBreakdown.AddModifiers(rule.StatModifiersAtTheMoment, true);
                        sb.AppendModifiersBreakdown("");
                    }

                    string bodyText = sb.ToString();
                    sb.Clear();

                    TooltipTemplateCombatLogMessage template = null;
                    if (!string.IsNullOrEmpty(bodyText))
                        template = new TooltipTemplateCombatLogMessage(headerStr, bodyText);

                    __result = new CombatLogMessage(headerLS, __instance.GetColor(), GameLogContext.GetIcon(), template, true);
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
