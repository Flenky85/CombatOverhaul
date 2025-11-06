using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class PhantasmalKillerAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PhantasmalKiller)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var outer = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var fort = (ContextActionSavingThrow)outer.Failed.Actions[0];
                    var inner = (ContextActionConditionalSaved)fort.Actions.Actions[0];
                    var dmg = (ContextActionDealDamage)inner.Succeed.Actions[0];

                    dmg.DamageType = new DamageTypeDescription
                    {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Magic
                    };
                    dmg.Value.DiceType = DiceType.D4;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default   
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 10,
                    m_AffectedByIntensifiedMetamagic = false
                })
                .SetDescriptionValue(
                    "You create a phantasmal image of the most fearsome creature imaginable to the subject simply by forming " +
                    "the fears of the subject's subconscious mind into something that its conscious mind can visualize: this " +
                    "most horrible beast. Only the spell's subject can see the phantasmal killer. You see only a vague shape. " +
                    "The target first gets a Will save to recognize the image as unreal. If that save fails, the phantasm " +
                    "touches the subject, and the subject must succeed at a Fortitude save or die from fear. Even if the " +
                    "Fortitude save is successful, the subject takes 1d4 points of damage per caster level (maximum 10d4)."
                )
                .Configure();
        }
    }
}
