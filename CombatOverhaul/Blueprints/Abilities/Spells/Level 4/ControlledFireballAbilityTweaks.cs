using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ControlledFireballAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ControlledFireball)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D6;
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Max = 8;                 
                })
                .Configure();
        }
    }
}
