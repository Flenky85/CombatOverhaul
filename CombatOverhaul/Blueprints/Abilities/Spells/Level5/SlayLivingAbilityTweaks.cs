using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class SlayLivingAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SlayLiving)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 12,
                    m_AffectedByIntensifiedMetamagic = false 
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var save = (ContextActionConditionalSaved)c.Actions.Actions[0];

                    var dmgOnSuccess = (ContextActionDealDamage)save.Succeed.Actions[0];
                    dmgOnSuccess.Value.DiceType = DiceType.D2;
                    dmgOnSuccess.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                    dmgOnSuccess.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var dmgOnFail = (ContextActionDealDamage)save.Failed.Actions[0];
                    dmgOnFail.Value.DiceType = DiceType.D10;
                    dmgOnFail.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                    dmgOnFail.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDescriptionValue(
                    "You can attempt to slay any one living creature. When you cast this spell, your hand seethes with eerie " +
                    "dark fire. You must succeed on a melee touch attack to touch the target. The target takes 1d10 per caster level " +
                    "(12d10 maximun) points of damage. If the target's Fortitude saving throw succeeds, it instead takes 1d2 per caster level " +
                    "(12d2 maximun) points of damage. The subject might die from damage even if it succeeds at its saving throw."
                )
                .Configure();
        }
    }
}
