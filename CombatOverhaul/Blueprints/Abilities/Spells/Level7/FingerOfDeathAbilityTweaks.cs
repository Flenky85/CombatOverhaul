using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class FingerOfDeathAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FingerOfDeath)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var dmgSucceed = (ContextActionDealDamage)cond.Succeed.Actions[0];
                    dmgSucceed.Value.DiceType = DiceType.D6;
                    dmgSucceed.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };
                    dmgSucceed.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var dmgFailed = (ContextActionDealDamage)cond.Failed.Actions[0];
                    dmgFailed.Value.DiceType = DiceType.D12;
                    dmgFailed.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    dmgFailed.Value.BonusValue = new ContextValue
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
                        rc.m_StepLevel = 0;
                        rc.m_UseMax = true;
                        rc.m_Max = 16;   
                        rc.m_AffectedByIntensifiedMetamagic = false;
                    },
                    rc => rc.name == "$AbilityRankConfig$465db832-dbea-4f15-83da-ba0f2f4b8a94"
                )
                .SetDescriptionValue(
                    "This spell instantly delivers 1d4 of damage per caster level (16d4 maximun). If the target's Fortitude saving throw succeeds, " +
                    "it instead takes 6d6 points of damage."
                )
                .Configure();
        }
    }
}
