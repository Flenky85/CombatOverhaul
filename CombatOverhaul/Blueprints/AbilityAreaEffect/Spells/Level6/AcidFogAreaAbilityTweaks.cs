using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Enums.Damage;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;


namespace CombatOverhaul.Blueprints.AbilityAreaEffect.Spells.Level6
{
    [AutoRegister]
    internal static class AcidFogAreaAbilityTweaks
    {
        public static void Register()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectGuids.AcidFogArea)
                .EditComponent<AbilityAreaEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Round.Actions[0]; 
                    dmg.DamageType.Type = DamageType.Energy;
                    dmg.DamageType.Energy = DamageEnergyType.Acid;         
                    dmg.Value.DiceType = DiceType.D6;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 4                                     
                    };
                })
                .Configure();
        }
    }
}
