using System;
using System.Text;
using HarmonyLib;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.Models.Log;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.Utility;
using UnityEngine;
using CombatOverhaul.Calculators;

namespace CombatOverhaul.Patches.UI.Roll
{
    [HarmonyPatch(typeof(AttackLogMessage), nameof(AttackLogMessage.GetData))]
    internal static class AttackLogMessage_GetData
    {
        static bool Prefix(AttackLogMessage __instance, RuleAttackRoll rule, ref CombatLogMessage __result)
        {
            using (ProfileScope.New("Build Attack Log Message [CO]", (UnityEngine.Object)null))
            {
                using (GameLogContext.Scope)
                {
                    if (!rule.Initiator.IsInGame || !rule.Target.IsInGame)
                    {
                        __result = null;
                    }

                    GameLogContext.SourceUnit = rule.Initiator;
                    GameLogContext.Target = rule.Target;
                    GameLogContext.Roll = rule.D20 + rule.AttackBonus;
                    GameLogContext.D20 = rule.D20;
                    GameLogContext.Modifier = rule.AttackBonus;
                    GameLogContext.DC = rule.TargetAC;

                    GameLogContext.AttackNumber = rule.AttackIndexForLog
                        ?? (rule.RuleAttackWithWeapon != null ? rule.RuleAttackWithWeapon.AttackNumber : -1);

                    GameLogContext.AttacksCount = rule.AttacksCountForLog
                        ?? (rule.RuleAttackWithWeapon != null ? rule.RuleAttackWithWeapon.AttacksCount : -1);

                    GameLogContext.SetConcealment(rule.ConcealmentCheck);
                    GameLogContext.SetMissChance(rule.MissChanceValue, rule.MissChanceRoll);
                    GameLogContext.Text = rule.Weapon.Wielder == null ? rule.Reason.Name : rule.Weapon.Name;

                    var headerSB = GameLogUtility.StringBuilder;
                    headerSB.Append(__instance.Message);

                    if (rule.IsCriticalConfirmed || rule.FortificationNegatesCriticalHit)
                    {
                        headerSB.Append(' ').Append(__instance.CritSuffix);
                    }
                    else if (rule.IsHit)
                    {
                        headerSB.Append(' ').Append(__instance.HitSuffix);
                    }
                    else if (rule.AutoMiss)
                    {
                        headerSB.Append(' ').Append(__instance.AutoMissSuffix);
                    }
                    else if (rule.D20 == 1)
                    {
                        headerSB.Append(' ');
                        string critMiss = __instance.CriticalMissSuffix;
                        if (!string.IsNullOrWhiteSpace(critMiss)) headerSB.Append(critMiss);
                        else headerSB.Append(__instance.MissSuffix);
                    }
                    else
                    {
                        headerSB.Append(' ').Append(__instance.MissSuffix);
                        if (rule.HitMirrorImageIndex > 0)
                        {
                            headerSB.Append(' ').Append(string.Format("({0})", __instance.MirrorImageSuffix));
                        }
                    }

                    if (rule.IsHit && (rule.IsSneakAttack || rule.FortificationNegatesSneakAttack))
                    {
                        headerSB.Append(' ').Append(__instance.SneakSuffix);
                    }

                    string headerText = headerSB.ToString();
                    headerSB.Clear();

                    string bodyText = BuildCustomBody(__instance, rule);

                    PrefixIcon icon = GameLogContext.GetIcon();
                    Color32 color = __instance.GetColor();

                    TooltipTemplateCombatLogMessage template = null;
                    if (!string.IsNullOrEmpty(bodyText))
                        template = new TooltipTemplateCombatLogMessage(headerText, bodyText);

                    __result = new CombatLogMessage(headerText, color, icon, template, true);
                    return false; 
                }
            }
        }

        private static string BuildCustomBody(AttackLogMessage msg, RuleAttackRoll rule)
        {
            var sb = new StringBuilder(256);

            int roll = rule.D20;
            int atkBonus = rule.AttackBonus;
            int ac = rule.TargetAC;

            var res = OpposedRollCore.ResolveD20(atkBonus, ac, roll);
            int pct = (int)Math.Round(res.P5 * 100.0f);
            int needed = res.TN;
            string hitText = rule.IsHit ? "hit" : roll == 1 ? "critical miss" : "miss";

            sb.Append("Attack roll: ").Append(roll).Append('\n')
              .Append("Chance of hit: ").Append(pct).Append("% (DC: ").Append(needed).Append(")\n")
              .Append("Result: ").Append(hitText);

            if (rule.IsCriticalRoll)
            {
                int critD20 = rule.CriticalConfirmationD20;
                int Acrit = rule.AttackBonus + rule.CriticalConfirmationBonus;
                int Dcrit = rule.TargetCriticalAC;

                var conf = OpposedRollCore.ResolveD20(Acrit, Dcrit, critD20);
                int pctCrit = (int)Math.Round(conf.P5 * 100.0f);
                int neededCrit = conf.TN;
                string confText = rule.IsCriticalConfirmed ? "confirmed" : "unconfirmed";

                sb.Append("\n\n")
                  .Append("Crit roll: ").Append(critD20).Append('\n')
                  .Append("Chance to confirm: ").Append(pctCrit).Append("% (").Append(neededCrit).Append(")\n")
                  .Append("Result: ").Append(confText);
            }

            if (rule.TargetUseFortification && rule.FortificationRoll > 0)
            {
                sb.Append("\n\n");
                GameLogContext.D100 = rule.FortificationRoll;
                GameLogContext.ChanceDC = rule.FortificationChance;
                if (rule.FortificationOvercomed) sb.Append(msg.FortificationFail);
                else sb.Append(msg.FortificationSuccess);
            }

            sb.AppendLine().AppendLine();
            msg.AppendAttackBonusBreakdown(sb, rule.AttackBonusRule);
            sb.AppendLine();
            msg.AppendArmorClassBreakdown(sb, rule.ACRule);

            return sb.ToString();
        }
    }
}
