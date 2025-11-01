using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ColorSprayAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ColorSpray)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];

                    static ContextActionDealDamage BuildForceDamage(bool half)
                    {
                        var dmg = new ContextActionDealDamage
                        {
                            DamageType = new DamageTypeDescription
                            {
                                Type = DamageType.Force,
                                Energy = DamageEnergyType.Magic
                            },
                            Value = new ContextDiceValue
                            {
                                DiceType = DiceType.D3,
                                DiceCountValue = new ContextValue
                                {
                                    ValueType = ContextValueType.Rank 
                                },
                                BonusValue = new ContextValue
                                {
                                    ValueType = ContextValueType.Simple,
                                    Value = 0
                                }
                            },
                            Half = half,             
                            HalfIfSaved = false,     
                            IsAoE = false
                        };
                        return dmg;
                    }

                    var succ = saved.Succeed.Actions?.ToList() ?? new System.Collections.Generic.List<GameAction>();
                    succ.Insert(0, BuildForceDamage(true));   
                    saved.Succeed.Actions = succ.ToArray();

                    var fail = saved.Failed.Actions?.ToList() ?? new System.Collections.Generic.List<GameAction>();
                    fail.Insert(0, BuildForceDamage(false));  
                    saved.Failed.Actions = fail.ToArray();
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 4; 
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (long)SpellDescriptor.Force;
                })
                .SetDescriptionValue(
                    "A vivid cone of clashing colors springs forth from your hand, causing creatures to become stunned, " +
                    "perhaps also blinded, and possibly knocked unconscious. Each creature within the cone is affected " +
                    "according to its HD.\n" +
                    "2 HD or less: The creature is blinded, stunned, and knocked unconscious for 2d4 rounds, then blinded " +
                    "and stunned for 1d4 rounds, and then stunned for 1 round. (Only living creatures are knocked unconscious.) " +
                    "3 or 4 HD: The creature is blinded and stunned for 1d4 rounds, then stunned for 1 round.5 or more HD: The " +
                    "creature is stunned for 1 round.\n" +
                    "Sightless creatures are not affected by color spray.In addition, each creature in the cone takes 1d3 force " +
                    "damage per caster level(maximum 4d3).A successful Will save halves this damage and negates the condition effects."
                )
                .Configure();
        }
    }
}
