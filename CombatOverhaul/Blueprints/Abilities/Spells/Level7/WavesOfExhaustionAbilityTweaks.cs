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
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class WavesOfExhaustionAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WavesOfExhaustion)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = new ContextActionDealDamage
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
                        Half = false,
                        HalfIfSaved = true,   
                        AlreadyHalved = false
                    };

                    var list = c.Actions.Actions.ToList();
                    list.Add(dmg);
                    c.Actions.Actions = list.ToArray();
                })
                .EditComponents<ContextRankConfig>(
                    rc =>
                    {
                        rc.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        rc.m_Progression = ContextRankProgression.AsIs;
                        rc.m_UseMax = true;
                        rc.m_Max = 16;
                        rc.m_StepLevel = 0;
                        rc.m_AffectedByIntensifiedMetamagic = false;
                    },
                    rc => rc.name == "$AbilityRankConfig$016f7cf4-af1f-4bf4-91b2-086281383c03"
                )
                .SetDescriptionValue(
                    "Waves of negative energy cause all living creatures in the spell's area to become exhausted. This spell " +
                    "has no effect on a creature that is already exhausted.\n" +
                    "Additionally, the targets takes 1d3 points of negative energy damage per caster level (maximum 16d3). A successful " +
                    "Will save halves this damage."
                )
                .Configure();
        }
    }
}
