using CombatOverhaul.Calculators;
using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.Localization;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.Models.Log;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.Utility;
using System;
using System.Text;

namespace CombatOverhaul.Patches.UI.Roll
{
    [HarmonyPatch(typeof(CombatManeuverLogMessage), nameof(CombatManeuverLogMessage.GetData))]
    internal static class CombatManeuverLogMessage_GetData
    {
        static bool Prefix(CombatManeuverLogMessage __instance, RuleCombatManeuver rule, ref CombatLogMessage __result)
        {
            using (ProfileScope.New("Build Combat Maneuver Log Message [CO]", (UnityEngine.Object)null))
            using (GameLogContext.Scope)
            {
                GameLogContext.SourceUnit = rule.Initiator;
                GameLogContext.Target = rule.Target;
                GameLogContext.Text = LocalizedTexts.Instance.CombatManeuver.GetText(rule.Type);
                GameLogContext.Roll = rule.InitiatorCMValue;
                GameLogContext.D20 = rule.InitiatorRoll;
                GameLogContext.Modifier = rule.InitiatorCMB;
                GameLogContext.DC = rule.TargetCMD;
                GameLogContext.SetConcealment(rule.ConcealmentCheck);

                LocalizedString resultMessage = rule.Result switch
                {
                    CombatManeuverResult.Success => __instance.MessageSuccess,
                    CombatManeuverResult.CriticalFail => __instance.MessageCriticalFail,
                    CombatManeuverResult.CriticalSuccess => __instance.MessageCriticalSuccess,
                    _ => __instance.MessageFail
                };

                var sb = GameLogUtility.StringBuilder;

                if (rule.InitiatorRoll != null)
                {
                    AppendCustomProbabilityBlock(sb, rule);
                    
                    if (rule.ConcealmentCheck != null)
                    {
                        __instance.AppendConcealmentResult(sb);
                    }
                    
                    if (rule.IsWeaponSnatcher)
                        __instance.AppendThieveryBonusBreakdown(sb, rule.ThieveryRoll);
                    else
                        __instance.AppendCombatManeuverBonusBreakdown(sb, rule.CMBRule);
                    __instance.AppendCombatManeuverDefenceBreakdown(sb, rule.CMDRule);
                }
                else
                {
                    __instance.AppendManeuverRollCancellationReason(sb, rule);
                }

                PrefixIcon icon = __instance.Icon != PrefixIcon.None ? __instance.Icon : GameLogContext.GetIcon();
                string bodyText = sb.ToString();
                sb.Clear();

                TooltipTemplateCombatLogMessage template = null;
                if (!string.IsNullOrEmpty(bodyText))
                    template = new TooltipTemplateCombatLogMessage(resultMessage, bodyText);

                __result = new CombatLogMessage(resultMessage, __instance.Color, icon, template, true);
                return false; 
            }
        }

        private static void AppendCustomProbabilityBlock(StringBuilder sb, RuleCombatManeuver rule)
        {
            
            int roll = (int)rule.InitiatorRoll;
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

            sb.Append("Maneuver roll: ").Append(roll).Append('\n')
              .Append("Chance of success: ").Append(pct).Append("% (DC: ").Append(needed).Append(")\n")
              .Append("Result: ").Append(resultText);
        }
    }
}
