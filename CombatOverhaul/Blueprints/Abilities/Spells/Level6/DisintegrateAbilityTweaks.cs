using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class DisintegrateAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Disintegrate)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var successDmg = (ContextActionDealDamage)saved.Succeed.Actions[0];
                    successDmg.Value.DiceType = DiceType.D6;
                    successDmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 5
                    };
                    successDmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    var failDmg = (ContextActionDealDamage)saved.Failed.Actions[0];
                    failDmg.Value.DiceType = DiceType.D12;
                    failDmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus
                    };
                    failDmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Type = AbilityRankType.DamageBonus;
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs; 
                    r.m_UseMax = true;
                    r.m_Max = 14;
                    r.m_AffectedByIntensifiedMetamagic = false; 
                })
                .SetDescriptionValue(
                    "A thin, green ray springs from your pointing finger.You must make a successful ranged touch attack to " +
                    "hit.Any creature struck by the ray takes 1d12 points of damage per caster level(to a maximum of 14d12). " +
                    "Any creature reduced to 0 or fewer hit points by this spell is entirely disintegrated, leaving behind " +
                    "only a trace of fine dust.A disintegrated creature's equipment is unaffected. The ray affects objects " +
                    "constructed entirely of force, such as forceful hand or a wall of force, but not magical effects such " +
                    "as a globe of invulnerability or an antimagic field.\n" +
                    "A creature or object that makes a successful Fortitude save is partially affected, taking only 5d6 points " +
                    "of damage.If this damage reduces the creature or object to 0 or fewer hit points, it is entirely disintegrated.\n" +
                    "Only the first creature or object struck can be affected; that is, the ray affects only one target per casting."
                )
                .Configure();
        }
    }
}
