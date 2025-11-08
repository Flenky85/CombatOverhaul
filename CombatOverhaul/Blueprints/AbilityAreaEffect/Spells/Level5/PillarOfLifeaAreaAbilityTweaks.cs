using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;


namespace CombatOverhaul.Blueprints.AbilityAreaEffect.Spells.Level5
{
    [AutoRegister]
    internal static class PillarOfLifeaAreaAbilityTweaks
    {
        public static void Register()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectGuids.PillarOfLifeaArea)
                .EditComponent<ContextRankConfig>(r =>
                {
                    if (r.m_Type == AbilityRankType.DamageDice)
                    {
                        r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        r.m_Progression = ContextRankProgression.AsIs;
                        r.m_UseMax = true;
                        r.m_Max = 12;             
                    }
                })
                .EditComponent<AbilityAreaEffectRunAction>(c =>
                {
                    var undeadGate = (Conditional)c.UnitEnter.Actions[0];

                    var undeadInner = (Conditional)undeadGate.IfTrue.Actions[0];
                    var dmg = (ContextActionDealDamage)undeadInner.IfTrue.Actions[0];

                    dmg.Value.DiceType = DiceType.D6;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                    dmg.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var livingGate = (Conditional)undeadGate.IfFalse.Actions[0];
                    var heal = (ContextActionHealTarget)livingGate.IfTrue.Actions[0];

                    heal.Value.DiceType = DiceType.D3;
                    heal.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                    heal.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .Configure();
        }
    }
}
