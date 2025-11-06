using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class VengefulCometsAttackAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.VengefulCometsAttack)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var actions = c.Actions.Actions;
                    var blunt = (ContextActionDealDamage)actions[0];
                    blunt.DamageType = new DamageTypeDescription
                    {
                        Type = DamageType.Physical,
                        Physical = new DamageTypeDescription.PhysicalData
                        {
                            Form = PhysicalDamageForm.Bludgeoning
                        }
                    };
                    blunt.Value = new ContextDiceValue
                    {
                        DiceType = DiceType.D8,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 1 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                    };

                    var cold = (ContextActionDealDamage)actions[1];
                    cold.DamageType = new DamageTypeDescription
                    {
                        Type = DamageType.Energy,
                        Energy = DamageEnergyType.Cold
                    };
                    cold.Value = new ContextDiceValue
                    {
                        DiceType = DiceType.D8,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                    };
                })
                .Configure();
        }
    }
}
