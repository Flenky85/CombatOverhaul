using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class WrackingRayAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WrackingRay)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.ProjectilesCount,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.DivStep,
                    m_StepLevel = 3,
                    m_UseMax = true,
                    m_Max = 4,                     
                    m_AffectedByIntensifiedMetamagic = false
                })
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 12,
                    m_AffectedByIntensifiedMetamagic = false
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var original = c.Actions.Actions;
                    var extra = new GameAction[original.Length + 1];
                    Array.Copy(original, extra, original.Length);

                    extra[original.Length] = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.NegativeEnergy
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D3,
                            DiceCountValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.Default  
                            },
                            BonusValue = new ContextValue
                            {
                                ValueType = ContextValueType.Simple,
                                Value = 0
                            }
                        },
                        IsAoE = false,
                        HalfIfSaved = true,
                        Half = false
                    };

                    c.Actions.Actions = extra;
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "A ray of sickly greenish-gray negative energy issues forth from the palm of your hand.\n" +
                    "Make a ranged touch attack against the target.A creature hit by this spell is wracked " +
                    "by painful spasms as its muscles and sinews wither and twist.\n" +
                    "The subject takes 1d4 points of Dexterity and Strength damage per 3 caster levels you " +
                    "possess(maximum 4d4 each).This spell cannot reduce an ability score below 1. A successful " +
                    "Fortitude save halves the damage.\n" +
                    "Additionally, the targets takes 1d3 points of negative energy damage per caster level (maximum 12d3). A successful " +
                    "Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
