using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class PolarRayAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PolarRay)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.Value.DiceType = DiceType.D8;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default,
                        Value = 0
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .EditComponents<ContextRankConfig>(
                    rc =>
                    {
                        rc.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        rc.m_Progression = ContextRankProgression.AsIs;
                        rc.m_UseMax = true;
                        rc.m_Max = 18;                        
                        rc.m_AffectedByIntensifiedMetamagic = false;
                        rc.m_StartLevel = 0;
                        rc.m_StepLevel = 0;
                    },
                    rc => rc.name == "$AbilityRankConfig$86b6314a-b0fa-4f63-a3b5-b0fd68e0bb52"
                )
                .SetDescriptionValue(
                    "A blue-white ray of freezing air and ice springs from your hand. You must succeed on a " +
                    "ranged touch attack with the ray to deal damage to a target. The ray deals 1d8 points of " +
                    "cold damage per caster level (maximum 18d6) and 1d4 points of Dexterity drain."
                )
                .Configure();
        }
    }
}
