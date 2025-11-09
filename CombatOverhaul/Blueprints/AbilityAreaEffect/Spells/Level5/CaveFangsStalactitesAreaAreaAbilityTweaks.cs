using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;


namespace CombatOverhaul.Blueprints.AbilityAreaEffect.Spells.Level5
{
    [AutoRegister]
    internal static class CaveFangsStalactitesAreaAreaAbilityTweaks
    {
        public static void Register()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectGuids.CaveFangsStalactitesArea)
                .EditComponent<AbilityAreaEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.UnitEnter.Actions[0];
                    var spawn = (ContextActionSpawnAreaEffect)cond.IfTrue.Actions[1];

                    spawn.DurationValue.Rate = DurationRate.Rounds;
                    spawn.DurationValue.DiceType = DiceType.D3;
                    spawn.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    spawn.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    spawn.DurationValue.m_IsExtendable = true;
                })
                .Configure();
        }
    }
}
