using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
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
    internal static class PoisonBreathAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PoisonBreath)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 16
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var acidDamage = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Acid
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
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
                        HalfIfSaved = true,                                   
                        IsAoE = false
                    };

                    var list = c.Actions.Actions?.ToList() ?? new System.Collections.Generic.List<GameAction>();
                    list.Insert(0, acidDamage);
                    c.Actions.Actions = list.ToArray();
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Acid;
                })
                .SetDescriptionValue(
                    "You expel a cone-shaped burst of toxic mist from your mouth, subjecting everyone " +
                    "caught in the area to a deadly poison, as per the poison spell.\n " +
                    "Additionally, the targets takes 1d4 points of acid damage per caster level (maximum 16d4). A successful " +
                    "Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
