using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
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
    internal static class TouchOfSlimeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TouchOfSlime)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 10;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = SavingThrowType.Fortitude;

                    var dmg = new ContextActionDealDamage
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
                        Half = false,
                        IsAoE = false
                    };

                    var existing = c.Actions?.Actions ?? new GameAction[0];
                    var newArr = new GameAction[existing.Length + 1];
                    newArr[0] = dmg;
                    System.Array.Copy(existing, 0, newArr, 1, existing.Length);
                    c.Actions = new ActionList { Actions = newArr };
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Acid;
                })
                .SetDescriptionValue(
                    "You create a coating of slime on your hand. When you make a successful melee touch attack with the slime, " +
                    "it pulls free of you and sticks to the target, at which point it acts like green slime, dealing 1d3 points of " +
                    "Constitution damage per round. Freezing, burning, and remove disease destroys this slime. It cannot transfer to " +
                    "a creature other than the original target, and it dies if separated from the original target or if the target dies. " +
                    "Only a living creature can be the target of this spell.\n" +
                    "Only a living creature can be the target of this spell.\n" +
                    "Additionally, the targets takes 1d4 points of acid damage per caster level (maximum 10d4). A successful " +
                    "Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
