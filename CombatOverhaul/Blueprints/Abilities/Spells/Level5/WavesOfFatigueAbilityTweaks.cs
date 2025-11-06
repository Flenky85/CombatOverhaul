using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class WavesOfFatigueAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WavesOfFatigue)
                .EditComponent<ContextRankConfig>(r =>
                {
                    if (r.m_BaseValueType == ContextRankBaseValueType.CasterLevel)
                    {
                        r.m_UseMax = true;
                        r.m_Max = 12;
                    }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.Actions.Actions[0];
                    var damage = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.NegativeEnergy
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
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
                        IsAoE = true,
                        HalfIfSaved = true,
                        Half = false
                    };

                    var save = new ContextActionSavingThrow
                    {
                        Type = SavingThrowType.Will,
                        Actions = new ActionList
                        {
                            Actions = new GameAction[] { damage }
                        }
                    };

                    var list = cond.IfTrue.Actions.ToList();
                    list.Add(save);
                    cond.IfTrue.Actions = list.ToArray();
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "Waves of negative energy render all living creatures in the spell's area fatigued. " +
                    "This spell has no effect on a creature that is already fatigued.\n" +
                    "Additionally, the targets takes 1d4 points of negative energy damage per caster level (maximum 12d4). A successful " +
                    "Will save halves this damage."
                )
                .Configure();
        }
    }
}
