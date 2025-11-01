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


namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class FlareBurstAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FlareBurst)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 4
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var original = c.Actions.Actions;
                    var saved = (ContextActionConditionalSaved)original[0];
                    var fireDamage = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Fire
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = true,
                    };

                    c.Actions.Actions = new GameAction[] { fireDamage, saved };

                    var failBuff = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    failBuff.DurationValue.Rate = DurationRate.Rounds;
                    failBuff.DurationValue.DiceType = DiceType.D3;
                    failBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    failBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Fire;
                })
                .SetDescriptionValue(
                    "A searing burst erupts in a 10-foot-radius from the chosen point. Each creature in the area takes 1d4 fire " +
                    "damage per caster level (maximum 4d4). A Fortitude save halves the damage; on a failed save, the creature is " +
                    "also dazzled for 2d3 rounds."
                )
                .Configure();
        }
    }
}
