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

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class SpitVenomAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AngelicAspect)
                .AddComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Type = AbilityRankType.Default;
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 8;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
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
                        IsAoE = false
                    };

                    var old = c.Actions.Actions;
                    var add = new GameAction[old.Length + 1];
                    add[0] = dmg;                     
                    System.Array.Copy(old, 0, add, 1, old.Length);
                    c.Actions.Actions = add;
                })
                .SetDescriptionValue(
                    "You spit a stream of venom at a target using a ranged touch attack. If the venom hits, it causes blindness for 1 round. " +
                    "The target must also save or be poisoned by black adder venom; the DC in successive rounds of the poison is equal to " +
                    "the spell's DC.\n" +
                    "Additionally, the target takes 1d4 points of acid damage per caster level(maximum 8d4). " +
                    "A successful Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
