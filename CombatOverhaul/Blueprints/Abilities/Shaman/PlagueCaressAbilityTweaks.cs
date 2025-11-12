using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class PlagueCaressAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PlagueCaressAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D3;    
                    apply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    apply.DurationValue.m_IsExtendable = false;

                    var dmg = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Acid
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D6,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = false,
                        AlreadyHalved = false,
                        IsAoE = false,
                        IgnoreCritical = true
                    };

                    c.Actions.Actions = new GameAction[] { dmg, apply };
                })
                .Configure();
        }
    }
}
