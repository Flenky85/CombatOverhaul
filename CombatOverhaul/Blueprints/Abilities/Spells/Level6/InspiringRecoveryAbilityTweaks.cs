using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class InspiringRecoveryAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.InspiringRecovery)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 14;
                    cfg.m_AffectedByIntensifiedMetamagic = false;
                })
                .EditComponent<AbilityEffectRunAction>(a =>
                {
                    var top = (Conditional)a.Actions.Actions[0];

                    var undeadCheck = (Conditional)top.IfFalse.Actions[0];
                    var heal = (ContextActionHealTarget)undeadCheck.IfFalse.Actions[0];
                    heal.Value.DiceType = DiceType.D4;
                    heal.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };
                    heal.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var save = (ContextActionSavingThrow)undeadCheck.IfTrue.Actions[0];
                    var dmg = (ContextActionDealDamage)save.Actions.Actions[0];
                    dmg.Value.DiceType = DiceType.D4;
                    dmg.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };
                    dmg.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var partyGate = (Conditional)top.IfTrue.Actions[0];           
                    var breath = (ContextActionBreathOfLife)partyGate.IfTrue.Actions[1];
                    breath.Value.DiceType = DiceType.D4;
                    breath.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };
                    breath.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDescriptionValue(
                    "You can heal a creature, harm an undead creature, or call upon a very recently dead creature " +
                    "to fight beyond death's reach. The target creature regains 1d4 hit points per caster levels (maximum 14d4).\n" +
                    "This healing can even bring back to life party members that have died within 2 rounds.If you " +
                    "cast this spell at a creature whose hit point total is at a negative amount less than its " +
                    "Constitution score, it comes back to life and stabilizes at its new hit point total.If you " +
                    "awaken a dead creature in this way, all allies within 60 feet who can see it regain consciousness " +
                    "gain a + 2 morale bonus on attack rolls, damage rolls, and saving throws for 6 rounds thereafter, " +
                    "as the healing powers of your deity have imbued them with renewed vigor.\n" +
                    "Creatures killed by death effects cannot be revived this way.This spell deals damage to undead " +
                    "creatures rather than curing them, and it cannot bring them back to life."
                )
                .Configure();
        }
    }
}
