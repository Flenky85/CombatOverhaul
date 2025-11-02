using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
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
    internal static class MortalTerrorAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MortalTerror)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageDice,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 6
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var top = c.Actions; 
                    var conditional = (Conditional)top.Actions[0];
                    var saving = (ContextActionSavingThrow)conditional.IfFalse.Actions[0];
                    var saved = (ContextActionConditionalSaved)saving.Actions.Actions[0];

                    var negEnergyDamage = new ContextActionDealDamage
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
                                ValueRank = AbilityRankType.DamageDice
                            },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = true,
                        Half = false,
                        IsAoE = false
                    };

                    saving.Actions = new ActionList
                    {
                        Actions = new GameAction[] { negEnergyDamage, saved }
                    };

                    var failBuff = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    failBuff.DurationValue.Rate = DurationRate.Rounds;
                    failBuff.DurationValue.DiceType = DiceType.D3;
                    failBuff.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    failBuff.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "You imbue the target with an exaggerated sense of its own mortality and a drive for self-preservation. The " +
                    "target is shaken, and the first time each round the target takes damage (including the first round of the " +
                    "spell’s duration), it must succeed at another Will save or its fear level increases by one step (from shaken " +
                    "to frightened). If the target fails a saving throw against this effect while frightened, it is transfixed in " +
                    "terror and is unconscious for the remainder of the spell’s duration. If the target succeeds at the initial " +
                    "Will save, it is shaken for 1 round but its fear level cannot be further increased by this spell’s effects " +
                    "during that round.\n" +
                    "Additionally, the spell deals 1d3 negative energy damage per caster level (maximum 6d3). " +
                    "On a successful save, the target takes half damage."
                )
                .Configure();
        }
    }
}
