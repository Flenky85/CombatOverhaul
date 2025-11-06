using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level2
{
    [AutoRegister]
    internal static class MoltenOrbAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MoltenOrb)
                
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 6
                })
                
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDiceAlternative,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 6
                })
                
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var conditional = (Conditional)c.Actions.Actions[0];

                    var mainHit = (ContextActionDealDamage)conditional.IfTrue.Actions[0];
                    mainHit.Value.DiceType = DiceType.D6;
                    mainHit.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                    
                    var saveBlock = (ContextActionSavingThrow)conditional.IfFalse.Actions[0];
                    var aoeHit = (ContextActionDealDamage)saveBlock.Actions.Actions[0];
                    aoeHit.Value.DiceType = DiceType.D4;
                    aoeHit.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDiceAlternative
                    };
                })
                .SetDescriptionValue(
                    "You create a fist-sized, red-hot ball of molten metal that you immediately hurl as a splash weapon. " +
                    "A direct hit deals 1d6 fire damage per caster level (maximum 6d6). Every creature within 5 feet of " +
                    "where the ball hits takes 1d4 fire damage per caster level (maximum 6d4); Reflex half. Each of these " +
                    "creatures takes an additional 1d6 fire damage each round for the next 1d3 rounds."
                )
                .Configure();
        }
    }
}
