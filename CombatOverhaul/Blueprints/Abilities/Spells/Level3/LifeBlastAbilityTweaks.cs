using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class LifeBlastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.LifeBlast)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.Actions.Actions[0];
                    var sv = (ContextActionSavingThrow)cond.IfTrue.Actions[0];
                    var dmg = (ContextActionDealDamage)sv.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D8;
                    dmg.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 8;                       
                })
                .SetDescriptionValue(
                    "When you cast this spell, you draw the life force from the surrounding land and hurl it at your enemies, " +
                    "dealing 1d8 points of positive energy damage per caster level (to a maximum of 8d8) to " +
                    "any undead creatures in the spell's area."
                )
                .Configure();
        }
    }
}
