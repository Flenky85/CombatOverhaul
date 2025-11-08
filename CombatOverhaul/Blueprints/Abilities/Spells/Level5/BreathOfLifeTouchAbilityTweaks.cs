using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Security.Policy;
using static HarmonyLib.Code;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;
using static Kingmaker.Blueprints.BlueprintAbilityResource;
using static Kingmaker.UnitLogic.Commands.UnitCommands;
using static Pathfinding.Util.RetainedGizmos;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class BreathOfLifeTouchAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BreathOfLifeTouch)
                .EditComponent<ContextRankConfig>(r =>
                {
                    if (r.m_Type == AbilityRankType.Default)
                    {
                        r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        r.m_Progression = ContextRankProgression.AsIs;
                        r.m_UseMax = true;
                        r.m_Max = 12;
                    }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond0 = (Conditional)c.Actions.Actions[0];
                    var cond1 = (Conditional)cond0.IfTrue.Actions[0];

                    var breath = (ContextActionBreathOfLife)cond1.IfTrue.Actions[1];
                    breath.Value.DiceType = DiceType.D4;
                    breath.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    breath.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var condAliveFalse = (Conditional)cond0.IfFalse.Actions[0];
                    var saving = (ContextActionSavingThrow)condAliveFalse.IfTrue.Actions[0];
                    var undeadDmg = (ContextActionDealDamage)saving.Actions.Actions[0];
                    undeadDmg.Value.DiceType = DiceType.D4;
                    undeadDmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    undeadDmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var heal = (ContextActionHealTarget)condAliveFalse.IfFalse.Actions[0];
                    heal.Value.DiceType = DiceType.D4;
                    heal.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    heal.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDescriptionValue(
                    "This spell cures 1d4 points of damage per caster level (maximum 12d4).\n" +
                    "Unlike other spells that heal damage, breath of life can bring recently slain creatures " +
                    "back to life.If cast upon a creature that has died within 2 rounds, apply the healing " +
                    "from this spell to the creature.If the healed creature's hit point total is at a negative " +
                    "amount less than its Constitution score, it comes back to life and stabilizes at its new " +
                    "hit point total. If the creature's hit point total is at a negative amount equal to or " +
                    "greater than its Constitution score, the creature remains dead.Creatures brought back to " +
                    "life through breath of life gain a temporary negative level that lasts for 1 day.\n" +
                    "Creatures slain by death effects cannot be saved by breath of life.\n" +
                    "Like cure spells, breath of life deals damage to undead creatures rather than curing them, and cannot bring them back to life."
                )
                .Configure();
        }
    }
}
